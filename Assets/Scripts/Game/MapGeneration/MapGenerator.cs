using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using MyCustomAttribute;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public enum Direction
    {
        top,
        right,
        bottom,
        left
    }
    public GameObject room;
    [SerializeField] private Transform roomPlace;
    [SerializeField] private GameObject playerRoom;
    [SerializeField] private GameObject bossRoom;
    [SerializeField] private GameObject unknownRoom;
    [SerializeField, Range(0, 4)] private int doorsOpenFisrtRoom;
    [SerializeField, MinMax(0, 1)] private Vector2 obstacleRateMinMax;
    [SerializeField, MinMax(0, 1)] private Vector2 EnemiesRateMinMax;
    private Vector2Int mapSize;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject corner;
    [SerializeField] private GameObject door;
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject[] enemies;
    [SerializeField, ReadOnly] private List<Transform> rooms;
    [SerializeField, ReadOnly] private int currentRoom;
    private float timer;
    private bool spawnDone;
    public static event Action OnSpawnDone;
    private Transform playerTracking;
    private Tile mapCenter;
    private List<Tile> bounder;
    private List<Tile> safeArea;
    private List<Tile> doorArea;
    private List<Tile> tiles;
    private Queue<Tile> shuffletiles;


#if UNITY_EDITOR
    [SerializeField] public bool showGizmos;
#endif

    private void Awake()
    {
        rooms = new List<Transform>();
    }

    private void OnEnable()
    {
        Room.OnSpawn += AddRoom;
        Room.OnUnSpawn += RemoveRoom;
    }

    private void OnDisable()
    {
        Room.OnSpawn -= AddRoom;
        Room.OnUnSpawn -= RemoveRoom;
    }

    private void Start()
    {

        Room firstRoom = Instantiate(room, transform.position, Quaternion.identity).GetComponent<Room>();
        firstRoom.OpenDoor(doorsOpenFisrtRoom, doorsOpenFisrtRoom);
        currentRoom = 0;
    }

    private void Update()
    {
        if (!spawnDone)
        {
            timer += Time.deltaTime;
            if (timer >= 0.5f)
            {
                SetRoom();
                spawnDone = true;
                OnSpawnDone?.Invoke();
            }

        }
    }

    public void AddRoom(Room room)
    {
        rooms.Add(room.transform);
        timer = 0;
    }

    public void RemoveRoom(Room room)
    {
        rooms.Remove(room.transform);
        timer = 0;
    }

    public void SetRoom()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == 0)
            {
                playerTracking = Instantiate(playerRoom, rooms[i].position, Quaternion.identity).transform;
            }
            else if (i == rooms.Count - 1)
            {
                Instantiate(bossRoom, rooms[i].position, Quaternion.identity);
            }
            else
            {
                Instantiate(unknownRoom, rooms[i].position, Quaternion.identity);
            }
        }
    }

    public void GenerateRoom(int seed)
    {
        Random.InitState(seed);
        tiles = new List<Tile>();
        bounder = new List<Tile>();
        safeArea = new List<Tile>();
        doorArea = new List<Tile>();
        mapSize = new Vector2Int(Random.Range(7, 11), Random.Range(7, 11));
        Transform holder = roomPlace.Find("room contents")?.transform;
        if (holder == null)
        {
            holder = new GameObject("room contents").transform;
            holder.SetParent(roomPlace);
        }

        ClearChildren(holder);

        mapCenter = new Tile(mapSize.x / 2, mapSize.y / 2);

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x * cellSize.x, 0, -mapSize.y / 2 + 0.5f + y * cellSize.x);
                Transform newFloor = Instantiate(floor, tilePosition, Quaternion.identity).transform;
                newFloor.gameObject.name = $"floor({x} - {y})";
                Tile tile = new Tile(x, y);
                tiles.Add(tile);

                //tạo viền ranh giới
                if (x == 0 || y == 0 || x == mapSize.x - 1 || y == mapSize.y - 1)
                {
                    bounder.Add(tile);
                }

                //tạo vùng cửa ra vào
                if ((x == mapCenter.x && (y == 0 || y == mapSize.y - 1)) || (y == mapCenter.y && (x == 0 || x == mapSize.x - 1)))
                {
                    doorArea.Add(tile);
                }

                //tạo vùng safe trc mỗi cửa ra vào
                if ((x >= mapCenter.x - 1 && x <= mapCenter.x + 1 && (y <= 1 || y >= mapSize.y - 2)) || (y >= mapCenter.y - 1 && y <= mapCenter.y + 1 && (x <= 1 || x >= mapSize.x - 2)))
                {
                    safeArea.Add(tile);
                }

                newFloor.parent = holder;
            }
        }

        shuffletiles = new Queue<Tile>(Utility.ShuffleArray<Tile>(tiles.ToArray()));

        PlacementWall(holder);
        PlacementObstacle(holder);
        GenerateNavMesh();
        PlacementEnemies(holder, seed);
    }

    private void PlacementObstacle(Transform holder)
    {
        if (obstacles.Length > 0)
        {
            float rate = Random.Range(obstacleRateMinMax.x, obstacleRateMinMax.y);
            int count = (int)((mapSize.x - 2) * (mapSize.y - 2) * rate);
            bool[,] obstacleMap = new bool[mapSize.x, mapSize.y];
            int currentObstacleCount = 0;
            for (int i = 0; i < count; i++)
            {
                Tile tile = shuffletiles.Dequeue();
                obstacleMap[tile.x, tile.y] = true;
                shuffletiles.Enqueue(tile);

                currentObstacleCount++;

                if (tile != mapCenter && !bounder.Contains(tile) && MapIsFullyAccessible(obstacleMap, currentObstacleCount))
                {
                    Vector3 pos = tile.GetPositon(mapSize, cellSize);
                    int rndIndex = Random.Range(0, obstacles.Length);
                    GameObject newObstacle = Instantiate(obstacles[rndIndex], pos, obstacles[rndIndex].transform.rotation, holder);
                }
                else
                {
                    obstacleMap[tile.x, tile.y] = false;
                    currentObstacleCount--;
                }
            }
        }
    }

    private void PlacementEnemies(Transform holder, int seed)
    {
        if (enemies.Length > 0)
        {
            float rate = Random.Range(EnemiesRateMinMax.x, EnemiesRateMinMax.y);
            int count = (int)((mapSize.x - 2) * (mapSize.y - 2) * rate);
            for (int i = 0; i < count; i++)
            {
                Tile tile = shuffletiles.Dequeue();
                shuffletiles.Enqueue(tile);
                if (!safeArea.Contains(tile))
                {
                    Vector3 pos = tile.GetPositon(mapSize, cellSize);
                    int rndIndex = Random.Range(0, enemies.Length);
                    GameObject newEnemy = Instantiate(enemies[rndIndex], pos + Vector3.up * 2f, enemies[rndIndex].transform.rotation, holder);
                }
            }
        }
    }
    private void PlacementWall(Transform holder)
    {
        foreach (Tile tile in bounder)
        {
            Quaternion rot = Quaternion.identity;
            Vector3 offset = Vector3.zero;

            //điều chỉnh vị trí tường ra rìa của phần ranh giới
            if (tile.x == 0)
            {
                offset = Vector3.left * cellSize.x / 2;
                rot = Quaternion.LookRotation(Vector3.right);
            }

            if (tile.x == mapSize.x - 1)
            {
                offset = Vector3.right * cellSize.x / 2;
                rot = Quaternion.LookRotation(Vector3.right);
            }

            if (tile.y == 0)
            {
                offset = Vector3.back * cellSize.y / 2;
            }

            if (tile.y == mapSize.y - 1)
            {
                offset = Vector3.forward * cellSize.y / 2;
            }

            //kiểm tra nếu là khu vực cửa thì tạo cửa nếu ko thì tạo tường
            if (doorArea.Contains(tile))
            {
                GameObject newDoor = Instantiate(door, tile.GetPositon(mapSize, cellSize) + offset, rot, holder);
                Door doorComp = newDoor.GetComponent<Door>();
                if (tile.x == mapCenter.x && tile.y == mapSize.y - 1)
                {
                    doorComp.direction = Direction.top;
                }
                else if (tile.x == mapCenter.x && tile.y == 0)
                {
                    doorComp.direction = Direction.bottom;
                }
                else if (tile.x == 0 && tile.y == mapCenter.y)
                {
                    doorComp.direction = Direction.left;
                }
                else
                {

                    doorComp.direction = Direction.right;
                }
                newDoor.name = $"door {doorComp.direction.ToString()}({tile.x} - {tile.y})";
            }
            else
            {
                //nếu ở 4 góc thì tạo corner
                if (tile.x == 0 && tile.y == 0 || tile.x == 0 && tile.y == mapSize.y - 1 || tile.x == mapSize.x - 1 && tile.y == 0 || tile.x == mapSize.x - 1 && tile.y == mapSize.y - 1)
                {
                    if (tile.x == 0 && tile.y == 0)
                    {
                        rot = Quaternion.LookRotation(Vector3.right);
                        offset = Vector3.back * cellSize.y / 2 + Vector3.left * cellSize.x / 2;
                    }
                    else if (tile.x == 0 && tile.y == mapSize.y - 1)
                    {
                        rot = Quaternion.LookRotation(Vector3.back);
                        offset = Vector3.forward * cellSize.y / 2 + Vector3.left * cellSize.x / 2;
                    }
                    else if (tile.x == mapSize.x - 1 && tile.y == 0)
                    {
                        rot = Quaternion.LookRotation(Vector3.forward);
                        offset = Vector3.back * cellSize.y / 2 + Vector3.right * cellSize.x / 2;
                    }
                    else
                    {
                        rot = Quaternion.LookRotation(Vector3.left);
                        offset = Vector3.forward * cellSize.y / 2 + Vector3.right * cellSize.x / 2;
                    }
                    Instantiate(corner, tile.GetPositon(mapSize, cellSize) + offset, rot, holder).name = $"corner({tile.x} - {tile.y})";
                }
                else
                {
                    Instantiate(wall, tile.GetPositon(mapSize, cellSize) + offset, rot, holder).name = $"wall({tile.x} - {tile.y})";
                }
            }

        }
    }

    private void GenerateNavMesh()
    {
        roomPlace.GetComponent<NavMeshSurface>().RemoveData();
        roomPlace.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private bool MapIsFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
    {
        bool[,] mapFlags = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];
        Queue<Tile> queue = new Queue<Tile>();
        queue.Enqueue(mapCenter);
        mapFlags[mapCenter.x, mapCenter.y] = true;
        int accessibleTileCount = 1;

        while (queue.Count > 0)
        {
            Tile tile = queue.Dequeue();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int neighbourX = tile.x + x;
                    int neighbourY = tile.y + y;
                    if (x == 0 || y == 0)
                    {
                        if (neighbourX >= 0 && neighbourX < obstacleMap.GetLength(0) && neighbourY >= 0 && neighbourY < obstacleMap.GetLength(1))
                        {
                            if (!mapFlags[neighbourX, neighbourY] && !obstacleMap[neighbourX, neighbourY])
                            {
                                mapFlags[neighbourX, neighbourY] = true;
                                queue.Enqueue(new Tile(neighbourX, neighbourY));
                                accessibleTileCount++;
                            }
                        }
                    }
                }
            }
        }

        int targetAccessibleTileCount = (int)(mapSize.x * mapSize.y - currentObstacleCount);
        return targetAccessibleTileCount == accessibleTileCount;
    }

    private void ClearChildren(Transform parent)
    {
        if (parent.childCount > 0)
        {
            while (parent.childCount != 0)
            {
                DestroyImmediate(parent.GetChild(0).gameObject);
            }
        }
    }


    public class Tile
    {
        public int x;
        public int y;
        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public Vector3 GetPositon(Vector2 mapSize, Vector2 cellSize)
        {
            return new Vector3(-mapSize.x / 2 + 0.5f + x * cellSize.x, 0, -mapSize.y / 2 + 0.5f + y * cellSize.x);
        }

        public static bool operator ==(Tile tile1, Tile tile2)
        {
            return tile1.x == tile2.x && tile1.y == tile2.y;
        }

        public static bool operator !=(Tile tile1, Tile tile2)
        {
            return !(tile1 == tile2);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!showGizmos) return;
        if (tiles != null)
        {
            foreach (Tile tile in tiles)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube(tile.GetPositon(mapSize, cellSize) + Vector3.up * 0.1f, new Vector3(cellSize.x, 0, cellSize.y));
            }
        }

        if (bounder != null)
        {
            foreach (Tile tile in bounder)
            {
                Gizmos.color = new Color(1, 0, 0, 0.8f);
                Gizmos.DrawCube(tile.GetPositon(mapSize, cellSize) + Vector3.up * 0.1f, new Vector3(cellSize.x, 0, cellSize.y));
            }
        }

        if (safeArea != null)
        {
            foreach (Tile tile in safeArea)
            {
                Gizmos.color = new Color(0, 1, 0, 0.8f);
                Gizmos.DrawCube(tile.GetPositon(mapSize, cellSize) + Vector3.up * 0.1f, new Vector3(cellSize.x, 0, cellSize.y));
            }
        }

        if (doorArea != null)
        {
            foreach (Tile tile in doorArea)
            {
                Gizmos.color = new Color(0, 0, 1, 0.8f);
                Gizmos.DrawCube(tile.GetPositon(mapSize, cellSize) + Vector3.up * 0.1f, new Vector3(cellSize.x, 0, cellSize.y));
            }
        }
    }
#endif
}

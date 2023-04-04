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
    [SerializeField, Range(0, 1)] private float outlineScale;
    [SerializeField] private int seed;
    [SerializeField] private int obstacleCount;
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private Vector2 cellSize;
    [SerializeField] private GameObject floor;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private Transform roomPlace;
    [SerializeField] private GameObject playerRoom;
    [SerializeField] private GameObject bossRoom;
    [SerializeField] private GameObject unknownRoom;
    [SerializeField, Range(0, 4)] private int doorsOpenFisrtRoom;
    [SerializeField, ReadOnly] private List<Transform> rooms;
    private float timer;
    private bool spawnDone;
    public static event Action OnSpawnDone;
    private Transform playerTracking;
    private List<Tile> tiles;
    private Queue<Tile> shuffletiles;

    private void Awake()
    {
        rooms = new List<Transform>();
        seed = Random.Range(0, 999999999);
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
                GenerateRoom();
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

    public void GenerateRoom()
    {
        tiles = new List<Tile>();
        Transform holder = new GameObject("grid").transform;
#if UNITY_EDITOR
        if (roomPlace.Find("grid"))
        {
            DestroyImmediate(roomPlace.Find("grid").gameObject);
        }
#endif
        holder.SetParent(roomPlace);


        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2 + 0.5f + x * cellSize.x, 0, -mapSize.y / 2 + 0.5f + y * cellSize.x);
                Transform newTile = Instantiate(floor, tilePosition, Quaternion.identity).transform;
                tiles.Add(new Tile(x, y));
                newTile.localScale = Vector3.one * (1 - outlineScale);
                newTile.parent = holder;
            }
        }

        shuffletiles = new Queue<Tile>(Utility.ShuffleArray<Tile>(tiles.ToArray(), seed));

        PlacementObstacle(obstacleCount, holder);
        GenerateNavMesh();
    }

    private void PlacementObstacle(int count, Transform holder)
    {
        for (int i = 0; i < count; i++)
        {
            Tile tile = shuffletiles.Dequeue();
            shuffletiles.Enqueue(tile);
            Vector3 pos = tile.GetPositon(mapSize, cellSize);
            GameObject newObstacle = Instantiate(obstacle, pos, obstacle.transform.rotation);
            newObstacle.transform.SetParent(holder);
        }
    }

    private void GenerateNavMesh() {
        roomPlace.GetComponent<NavMeshSurface>().RemoveData();
        roomPlace.GetComponent<NavMeshSurface>().BuildNavMesh();
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

        public Vector3 GetPositon(Vector2 mapSize, Vector2 cellSize)
        {
            return new Vector3(-mapSize.x / 2 + 0.5f + x * cellSize.x, 0, -mapSize.y / 2 + 0.5f + y * cellSize.x);
        }
    }
}

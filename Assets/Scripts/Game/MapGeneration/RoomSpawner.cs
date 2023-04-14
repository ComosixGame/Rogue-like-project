using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public MapGenerator.Direction direction;
    [SerializeField] private GameObject door;
    public bool close = true;
    public bool spawn = true;
    private MapGenerator mapGenerator;
    private Room room;

    private void Awake() {
        room = GetComponentInParent<Room>();
    }

    private void OnEnable() {
        MapGenerator.OnSpawnDone += CheckNeighborhood;
    }

    private void OnDisable() {
        MapGenerator.OnSpawnDone -= CheckNeighborhood;
    }

    void Start()
    {
        gameObject.SetActive(!close);
        door.SetActive(close);
        mapGenerator = GameObject.FindGameObjectWithTag("Rooms").GetComponent<MapGenerator>();
        Invoke("AddToQueueSpawn", 0.1f);
    }

    private void AddToQueueSpawn() {
        if(gameObject.activeSelf) {
            mapGenerator.roomSpawners.Enqueue(this);
        }
    }

    public void Spawn()
    {
        if (!close && spawn)
        {
            Room newRoom = Instantiate(mapGenerator.room, transform.position, Quaternion.identity, mapGenerator.mapHolder).GetComponent<Room>();
            newRoom.Connect(GetConnect(direction));
        }
        // gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //check already have room
        if (other.TryGetComponent(out Room room))
        {
            spawn = false;
        }
    }

    private void CheckNeighborhood()
    {
        Collider[] results = new Collider[10];
        int numFound = Physics.OverlapBoxNonAlloc(transform.position, Vector3.one , results);
        for (int i = 0; i < numFound; i++)
        {
            if(results[i].TryGetComponent(out Room roomNeighborhood)) {
                if(!roomNeighborhood.openDirections.Contains(GetConnect(direction))) {
                    close = true;
                    gameObject.SetActive(!close);
                    door.SetActive(close);
                    room.openDirections.Remove(direction);
                } else {
                    switch(direction) {
                        case MapGenerator.Direction.left:
                            room.connectedRooms[0] = roomNeighborhood;
                            break;
                        case MapGenerator.Direction.top:
                            room.connectedRooms[1] = roomNeighborhood;
                            break;
                        case MapGenerator.Direction.right:
                            room.connectedRooms[2] = roomNeighborhood;
                            break;
                        default:
                            room.connectedRooms[3] = roomNeighborhood;
                            break;
                    }
                }
            }
        }
    }

    private MapGenerator.Direction GetConnect(MapGenerator.Direction direction)
    {
        switch (direction)
        {
            case MapGenerator.Direction.top:
                return MapGenerator.Direction.bottom;

            case MapGenerator.Direction.right:
                return MapGenerator.Direction.left;

            case MapGenerator.Direction.bottom:
                return MapGenerator.Direction.top;

            default:
                return MapGenerator.Direction.right;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}

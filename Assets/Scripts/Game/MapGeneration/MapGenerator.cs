using System;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;

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
    [SerializeField] private GameObject playerRoom;
    [SerializeField] private GameObject bossRoom;
    [SerializeField] private GameObject unknownRoom;
    [SerializeField, Range(0, 4)] private int doorsOpenFisrtRoom;
    [SerializeField, ReadOnly] private List<Transform> rooms;
    private float timer;
    private bool spawnDone;
    public static event Action OnSpawnDone;
    private Transform playerTracking;

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
        for(int i = 0; i < rooms.Count; i++) {
            if(i == 0) {
                playerTracking = Instantiate(playerRoom, rooms[i].position, Quaternion.identity).transform;
            } else if(i == rooms.Count - 1) {
                Instantiate(bossRoom, rooms[i].position, Quaternion.identity);
            } else {
                Instantiate(unknownRoom, rooms[i].position, Quaternion.identity);
            }
        }
    }
}

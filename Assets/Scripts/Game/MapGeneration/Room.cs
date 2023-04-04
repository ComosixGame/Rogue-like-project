using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Room : MonoBehaviour
{
    public List<RoomSpawner> roomSpawners = new List<RoomSpawner>();
    public List<MapGenerator.Direction> openDirections = new List<MapGenerator.Direction>();
    public bool spawned;
    public static event Action<Room> OnSpawn;
    public static event Action<Room> OnUnSpawn;

    private void OnEnable()
    {
        OnSpawn?.Invoke(this);
    }

    private void OnDisable()
    {
        OnUnSpawn?.Invoke(this);
    }

    public void Connect(MapGenerator.Direction direction)
    {
        RoomSpawner roomSpawner = roomSpawners.Find((roomSpawner => roomSpawner.direction == direction));
        roomSpawner.close = false;
        roomSpawner.spawn = false;
        openDirections.Add(direction);
        roomSpawners.Remove(roomSpawner);
        OpenDoor(0, 1);
        spawned = true;
    }

    public void OpenDoor(int minDoors, int maxDoors)
    {
        int doorsOpen = Random.Range(minDoors, maxDoors + 1);
        for (int i = 0; i < doorsOpen; i++)
        {
            int index = Random.Range(0, roomSpawners.Count);
            roomSpawners[index].close = false;
            roomSpawners[index].spawn = true;
            openDirections.Add(roomSpawners[index].direction);
            roomSpawners.Remove(roomSpawners[index]);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //destroy if postion already have room
        if (other.TryGetComponent(out Room room))
        {
            if(!spawned && room.spawned) {
                Destroy(gameObject);
            } 
        }
    }
}

using UnityEngine;

/**
 * Của lên kết giữa các phòng trigger lại việc generate phòng mới
 */

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    public MapGenerator mapGenerator;
    public MapGenerator.Direction direction;
    public Room connectRoom;

    private void OnEnable() {
        MapGenerator.OnRoomClear += OpenDoor;
    }

    private void OnDisable() {
        MapGenerator.OnRoomClear -= OpenDoor;
    }
    
    private void OpenDoor() {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
        {
            mapGenerator.NextRoom(connectRoom, direction, other.transform);
        }
    }
}

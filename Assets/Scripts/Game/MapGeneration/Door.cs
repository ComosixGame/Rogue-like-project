using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Door : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    public MapGenerator mapGenerator;
    public MapGenerator.Direction direction;
    public Room connectRoom;

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
        {
            mapGenerator.NextRoom(connectRoom, direction, other.transform);
        }
    }
}

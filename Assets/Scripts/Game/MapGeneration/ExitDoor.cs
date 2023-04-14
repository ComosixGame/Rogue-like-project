using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ExitDoor : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    public MapGenerator mapGenerator;

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & (1 << other.gameObject.layer)) != 0)
        {
            mapGenerator.NextLevel();
        }
    }
}

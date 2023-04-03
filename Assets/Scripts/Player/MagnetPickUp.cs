using UnityEngine;

public class MagnetPickUp : MonoBehaviour
{
    [SerializeField] private Vector3 center;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;

    
    private void Update() {
        Vector3 offset = transform.position + center;
        Collider[] hitColliders = Physics.OverlapSphere(offset, radius, layer);
        foreach (Collider hitCollider in hitColliders)
        {
            if(hitCollider.GetComponent<AbsItemObjectPool>().readlyPickup) {
                Transform item = hitCollider.transform;
                item.position = Vector3.MoveTowards(item.position, offset, 20f * Time.deltaTime);
            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + center, radius);
    }
#endif
}

using UnityEngine;

public class MagnetPickUp : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;
    
    private void Update() {
        Vector3 center = transform.position + Vector3.up * offset;
        Collider[] hitColliders = Physics.OverlapSphere(center, radius, layer);
        foreach (Collider hitCollider in hitColliders)
        {
            Transform item = hitCollider.transform;
            item.position = Vector3.MoveTowards(item.position, center, 10f * Time.deltaTime);
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * offset, radius);
    }
#endif
}

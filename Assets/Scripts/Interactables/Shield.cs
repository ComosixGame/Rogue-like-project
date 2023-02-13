using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Shield : MonoBehaviour
{
    [SerializeField] private float speedRotate;
    public Transform shield;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * speedRotate);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }

    private void OnDrawGizmosSelected() {
#if UNITY_EDITOR
        Handles.color = new Color32(66, 133, 244, 80);
        Handles.DrawSolidDisc(transform.position, Vector3.up, Vector3.Distance(transform.position, shield.position));
#endif
    }
}

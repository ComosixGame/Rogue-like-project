using UnityEngine;
using UnityEngine.UI;

public class PlayerTracking : MonoBehaviour
{
    private bool move;
    private Vector3 target;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject FullMap, MiniMap;
    [SerializeField] private Slider zoomSlider;
    private float zoomOrigin;

    private void Awake() {
        zoomOrigin = cam.orthographicSize;
    }

    private void Start() {
        zoomSlider.onValueChanged.AddListener(ZoomMap);
    }

    private void OnDisable() {
        zoomSlider.onValueChanged.RemoveAllListeners();
    }

    public void MoveTo(Vector3 position) {
        move = true;
        target = position;
    }

    private void Update() {
        if(move) {
            transform.position = Vector3.Lerp(transform.position, target, 5 * Time.deltaTime );
            if(Vector3.Distance(transform.position, target) <= 0.1f) {
                move = false;
            } 
        }
    }

    public void ZoomMap(float value) {
        cam.orthographicSize = zoomOrigin / value;
    }

    public void OpenFullMap(bool open) {
        FullMap.SetActive(open);
        MiniMap.SetActive(!open);
        ZoomMap(zoomSlider.value);
        if(!open) cam.orthographicSize = zoomOrigin;
    }
}

using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Collider))]
public class NextLevelDoor : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private MapGeneration mapGeneration;
    public GameObject door;
    public Vector3 playerStartPosition;
    private bool levelCleared;

    private void Awake() {
        mapGeneration.OnLevelCleared += OnlevelCleared;
    }

    private void OnTriggerEnter(Collider other) {
        if((layer & (1<<other.gameObject.layer)) != 0) {
            if(levelCleared) {
                StartCoroutine(CoroutineNextlevel(other.transform));
            }
        }
    }

    private void OnlevelCleared() {
        door.SetActive(true);
        levelCleared =  true;
    }

    IEnumerator CoroutineNextlevel(Transform player) {
        yield return new WaitForSeconds(0.5f);
        NextLevel(player);
    }

    private void NextLevel(Transform player) {
        door.SetActive(false);
        mapGeneration.NextLevel();
        CharacterController controller = player.GetComponent<CharacterController>();
        controller.enabled = false;
        player.position = playerStartPosition;
        controller.enabled = true;
    }

#if UNITY_EDITOR

[CustomEditor(typeof(NextLevelDoor))]
public class NextLevelDoorEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        
    }
    private void OnSceneGUI(){
        NextLevelDoor nextLevel = target as NextLevelDoor;
        Handles.Label(nextLevel.playerStartPosition, $"playerStartPosition", "TextField");
        EditorGUI.BeginChangeCheck();
        Vector3 pos = Handles.PositionHandle(nextLevel.playerStartPosition, Quaternion.identity);
        if(EditorGUI.EndChangeCheck()) {
            UnityEditor.Undo.RecordObject(nextLevel, "Update playerStartPosition");
            nextLevel.playerStartPosition = pos;
        }
    }
}
#endif

}

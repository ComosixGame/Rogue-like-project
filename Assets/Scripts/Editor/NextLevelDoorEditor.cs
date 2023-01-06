using UnityEngine;
using UnityEditor;

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

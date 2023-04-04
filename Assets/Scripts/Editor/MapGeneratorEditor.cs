using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    private bool livePre;
    public override void OnInspectorGUI() {
        livePre = EditorGUILayout.Toggle("Live Preview", livePre);
        base.OnInspectorGUI();
        MapGenerator map = target as MapGenerator;
        if(livePre) {
            map.GenerateRoom();
        }
    }
}

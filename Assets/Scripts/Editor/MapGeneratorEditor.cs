using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        MapGenerator map = target as MapGenerator;
        EditorGUILayout.Space(20);
        if(GUILayout.Button("GENERATE", GUILayout.Height(50))) {
            map.GenerateRoom(UnityEngine.Random.Range(Int32.MinValue, Int32.MaxValue));
        }
    }
}

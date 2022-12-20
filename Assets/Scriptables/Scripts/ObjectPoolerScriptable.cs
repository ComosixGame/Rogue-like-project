using System;
using System.Collections.Generic;
using UnityEngine;
using MyCustomAttribute;


[CreateAssetMenu(fileName = "New ObjectPooler Manager", menuName = "ObjectPooler Manager")]
public class ObjectPoolerScriptable : ScriptableObject
{
    [System.Serializable]
    public class ScripblePrefab {
        public string key;
        public int size;    
        public GameObject prefab;
    }
    public ScripblePrefab[] prefabs;
}

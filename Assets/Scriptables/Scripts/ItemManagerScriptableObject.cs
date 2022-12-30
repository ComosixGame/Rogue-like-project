using UnityEngine;
using MyCustomAttribute;

[CreateAssetMenu(fileName = "New Item Manager", menuName = "Scriptable Manager/Item Manager")]
public class ItemManagerScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class ItemPrefab {
        public AbsItemObjectPool itemObjectPool;
        [Label("Chance To Spawn(%)")] public float chanceToSpawn;
    }

    public ItemPrefab[] itemPrefabs;
}

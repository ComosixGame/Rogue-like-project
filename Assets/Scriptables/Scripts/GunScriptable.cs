using UnityEngine;

[CreateAssetMenu(fileName = "GunScripable Manager", menuName = "Scriptable Manager/Gun Manager")]
public class GunScriptable : ScriptableObject
{
    [System.Serializable]
    public class Gun{
        public string nameGun;
        public int priceGun;
        public GameObject prefabGun;
    }
    
    public Gun[] guns;
}

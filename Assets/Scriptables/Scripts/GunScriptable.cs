using UnityEngine;

[CreateAssetMenu(fileName = "GunScripable Manager", menuName = "Scriptable Manager/Gun Manager")]
public class GunScriptable : ScriptableObject
{
    [System.Serializable]
    public class Gun{
        public int index;
        public string nameGun;
    }
    
    public Gun[] guns;
}

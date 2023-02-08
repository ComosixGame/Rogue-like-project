using UnityEngine;


[CreateAssetMenu(fileName = "CharacterScripable Manager", menuName = "Scriptable Manager/Character Manager")]
public class CharacterScripable : ScriptableObject
{
    [System.Serializable]
    public class Character{
        public string nameCharacter;
        public GameObject prefab;
    }

    public Character[] characters;
}

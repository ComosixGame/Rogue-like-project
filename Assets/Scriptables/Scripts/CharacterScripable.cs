using UnityEngine;


[CreateAssetMenu(fileName = "CharacterScripable Manager", menuName = "Scriptable Manager/Character Manager")]
public class CharacterScripable : ScriptableObject
{
    [System.Serializable]
    public class Character{
        public string nameCharacter;
        public int priceCharacter;
        public GameObject prefab;
        public Sprite thumb;
    }

    public Character[] characters;
}

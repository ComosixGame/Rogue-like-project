using UnityEngine;

[CreateAssetMenu(fileName = "ChapterScriptAble Manager", menuName = "Scriptable Manager/Chapter Manager")]
public class ChapterScriptAble : ScriptableObject
{
    [System.Serializable]
    public class Chapter{
        public int index;
        public string path;
        public string nameChapter;
        public Sprite thumb;
    }

    public Chapter[] chapters;
}

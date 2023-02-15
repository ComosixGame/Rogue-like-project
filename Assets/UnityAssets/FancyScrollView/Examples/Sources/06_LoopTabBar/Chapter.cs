using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06
{
    class Chapter : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition transition = default;
        public int index;
        public string nameChapter;
        public Sprite thumb;

        Chapter currentChapter = null;
        public void In(MovementDirection direction) => transition?.In(direction);

        public void Out(MovementDirection direction) => transition?.Out(direction);

        private void Awake() {
            currentChapter = GetComponent<Chapter>();
        }

        private void Start() {
            gameObject.GetComponentInParent<Image>().sprite = currentChapter.thumb;
        }
    }
}

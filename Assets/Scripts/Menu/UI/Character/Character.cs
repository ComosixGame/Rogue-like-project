using UnityEngine;
using UnityEngine.UI;

namespace FancyScrollView.Example06{
    public class Character : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition transition = default;

        public int index;
        public int priceCharacter;
        public string nameCharacter;
        public Sprite thumb;

        [SerializeField] private Text nameCharacterText;

        public void In(MovementDirection direction) => transition?.In(direction);

        public void Out(MovementDirection direction) => transition?.Out(direction);

        Character currentCharacter = null;

        private void Awake() {
            currentCharacter = GetComponent<Character>();
        }
       
        private void Start() {
            gameObject.GetComponent<Image>().sprite = currentCharacter.thumb;
        }

    }
}

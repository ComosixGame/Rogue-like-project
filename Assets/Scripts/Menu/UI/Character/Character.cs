using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace FancyScrollView.Example06{
    public class Character : MonoBehaviour
    {
        [SerializeField] SlideScreenTransition transition = default;

        public int index;
        public int priceCharacter;
        public string nameCharacter;
        public Sprite thumb;
        [SerializeField] public TMP_Text nameCharacterText;
        [SerializeField] public TMP_Text priceCharacterText;
        [SerializeField] public Image spriteCharacter;

        public void In(MovementDirection direction) => transition?.In(direction);

        public void Out(MovementDirection direction) => transition?.Out(direction);

        Character currentCharacter = null;

        private void Awake() {
            currentCharacter = GetComponent<Character>();
            spriteCharacter.GetComponent<Image>();
        }
       
        private void Start() {
            //gameObject.GetComponent<Image>().sprite = currentCharacter.thumb;
            currentCharacter.spriteCharacter.sprite = currentCharacter.thumb;
            currentCharacter.nameCharacterText.text = $"{currentCharacter.nameCharacter}";
            currentCharacter.priceCharacterText.text = $"{currentCharacter.priceCharacter}";
        }

    }
}

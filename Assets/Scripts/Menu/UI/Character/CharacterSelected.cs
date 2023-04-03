using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace FancyScrollView.Example06 {
    public class CharacterSelected : MonoBehaviour
    {
        [SerializeField] ScrollView scrollView = default;
        [SerializeField] Text selectedItemInfo = default;
        [SerializeField] Text priceCharacter;
        [SerializeField] Sprite iconCharacter;
        [SerializeField] List<Character> charactersList = new List<Character>();
        [SerializeField] CharacterScripable characterScripable;
        [SerializeField] private Transform CharacterParent;
        [SerializeField] GraphicRaycaster graphicRaycasterAdd;
        Character currentCharacter;
        [SerializeField] private Character characterPrefab;
        public GameObject _popupSelectedCharacter;
        [SerializeField] GameObject _popUpSelectedCharacter;
        [SerializeField] private GameObject _popupConfirm;
        [SerializeField] private GameObject _btnBuy;
        [SerializeField] private GameObject _btnSelect;
        
        //singleton
        private LoadSceneManager loadSceneManager;
        private GameManager gameManager;
        private SoundManager soundManager;

        //action
        public static event Action ConfirmSelected;

        //sound
        public AudioClip btnSound;

        private void Awake() {
            gameManager = GameManager.Instance;
            soundManager = SoundManager.Instance;
        }

        void Start()
        {
            for(int i = 0; i<characterScripable.characters.Length; i++) {
                Character characterDisplay = Instantiate(characterPrefab);
                charactersList.Add(characterDisplay);
                characterDisplay.gameObject.SetActive(i == 0);
                characterDisplay.GetComponent<SlideScreenTransition>().graphicRaycaster = graphicRaycasterAdd;
                characterDisplay.transform.SetParent(CharacterParent, false);
                characterDisplay.index = i;
                characterDisplay.priceCharacter = characterScripable.characters[i].priceCharacter;
                characterDisplay.nameCharacter = characterScripable.characters[i].nameCharacter;
                characterDisplay.thumb = characterScripable.characters[i].thumb;
            }

            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, charactersList.Count)
                .Select(i => new ItemData($"{characterScripable.characters[i].nameCharacter}"))
                .ToList();
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }

       void OnSelectionChanged(int index, MovementDirection direction)
        {

            if (currentCharacter != null)
            {
                currentCharacter.Out(direction);
                currentCharacter = null;
            }

            if (index >= 0 && index < charactersList.Count)
            {
                currentCharacter = charactersList[index];
                currentCharacter.In(direction);
            }

            bool own = gameManager.characterOwn.IndexOf(currentCharacter.index) != -1;
            
            _btnBuy.SetActive(!own);
            _btnSelect.SetActive(own);

            //update UI
            selectedItemInfo.text = $"Selected character: {currentCharacter.nameCharacter}";
            priceCharacter.text = $"Price character: {currentCharacter.priceCharacter}";
            gameObject.transform.GetChild(4).GetChild(0).GetComponent<Image>().sprite  = currentCharacter.thumb;
        }

        public void SelectedCharacter(){
            gameManager.SelectedCharacter(currentCharacter.index);
            ConfirmSelected?.Invoke();
        }

        public void BuyItem() {
            bool success = gameManager.BuyItem(currentCharacter.index, currentCharacter.priceCharacter);
            _popupConfirm.SetActive(!success);
            _btnBuy.SetActive(!success);
            _btnSelect.SetActive(success);
        }

        public void PlayBtnSound(){
            soundManager.PlaySound(btnSound);
        }
    }
}

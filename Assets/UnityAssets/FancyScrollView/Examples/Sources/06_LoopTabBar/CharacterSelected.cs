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
        [SerializeField] List<Character> charactersList = new List<Character>();
        [SerializeField] CharacterScripable characterScripable;
        [SerializeField] private Transform CharacterParent;
        [SerializeField] GraphicRaycaster graphicRaycasterAdd;
        Character currentCharacter;
        [SerializeField] private Character characterPrefab;
        private LoadSceneManager loadSceneManager;
        public GameObject _popupSelectedCharacter;
        //[SerializeField] Text nameCharacter;
        [SerializeField] GameObject _popUpSelectedCharacter;
        private GameManager gameManager;
        [SerializeField] private GameObject _popupConfirm;

        [SerializeField] private GameObject _btnBuy;
        [SerializeField] private GameObject _btnSelect;
        private void Awake() {
            gameManager = GameManager.Instance;
            // _popUpSelectedCharacter.SetActive(false);
        }

        void Start()
        {
            for(int i = 0; i<characterScripable.characters.Length; i++) {
                Character characterDisplay = Instantiate(characterPrefab);
                charactersList.Add(characterDisplay);
                characterDisplay.GetComponent<SlideScreenTransition>().graphicRaycaster = graphicRaycasterAdd;
                characterDisplay.transform.SetParent(CharacterParent, false);
                characterDisplay.index = i;
                characterDisplay.priceCharacter = characterScripable.characters[i].priceCharacter;
                characterDisplay.nameCharacter = characterScripable.characters[i].nameCharacter;
            }

            scrollView.OnSelectionChanged(OnSelectionChanged);

            var items = Enumerable.Range(0, charactersList.Count)
                .Select(i => new ItemData($"Tab {i}"))
                .ToList();
            scrollView.UpdateData(items);
            scrollView.SelectCell(0);
        }

       void OnSelectionChanged(int index, MovementDirection direction)
        {
            selectedItemInfo.text = $"Selected character: index {index}";

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


            //Debug.Log(characterScripable.characters[gameManager.characterSeleted].nameCharacter);
        }

        public void SelectedCharacter(){
            gameManager.SelectedCharacter(currentCharacter.index);
            _popUpSelectedCharacter.SetActive(false);
        }

        public void BuyItem() {
           bool success = gameManager.BuyItem(currentCharacter.index, currentCharacter.priceCharacter);
            _popupConfirm.SetActive(!success);
            _btnBuy.SetActive(!success);
            _btnSelect.SetActive(success);
            //_popUpSelectedCharacter.SetActive(false);
        }
    }
}

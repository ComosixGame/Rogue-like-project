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
        [SerializeField] Text characterIndex;
        [SerializeField] Text priceCharacter;
        [SerializeField] GameObject _popUpSelectedCharacter;
        private GameManager gameManager;
        private PlayerData playerData;
        
        private void Awake() {
            gameManager = GameManager.Instance;
            playerData = PlayerData.Load();
            _popUpSelectedCharacter.SetActive(false);
        }

        void Start()
        {
            for(int i = 0; i<characterScripable.characters.Length; i++) {
                Character characterDisplay = Instantiate(characterPrefab);
                charactersList.Add(characterDisplay);
                characterDisplay.GetComponent<SlideScreenTransition>().graphicRaycaster = graphicRaycasterAdd;
                characterDisplay.transform.SetParent(CharacterParent, false);
                characterDisplay.index = i;
                characterDisplay.price = characterScripable.characters[i].priceCharacter;
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
        }

        public void SelectedCharacter(){
            int coinCurrent = playerData.coin;
            int priCharacter = currentCharacter.price;
            if(coinCurrent >= priCharacter){
                _popupSelectedCharacter.SetActive(false);
                characterIndex.text = $"Character Index: {currentCharacter.index}";
                priceCharacter.text = $"Price character: {currentCharacter.price}";
                gameManager.SelectedCharacter(currentCharacter.index);
                gameManager.updateCoinThenBuyItem(priCharacter);
            }else{
                _popUpSelectedCharacter.SetActive(true);
            }
        }

        public void handleConfirmSelectedCharacter(){
            _popUpSelectedCharacter.SetActive(false);
        }
    }
}

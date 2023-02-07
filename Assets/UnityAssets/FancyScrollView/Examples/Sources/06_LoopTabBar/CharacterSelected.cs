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

        void Start()
        {
            foreach(CharacterScripable.Character character in characterScripable.characters){
                Character characterDisplay = Instantiate(characterPrefab);
                charactersList.Add(characterDisplay);
                characterDisplay.GetComponent<SlideScreenTransition>().graphicRaycaster = graphicRaycasterAdd;
                characterDisplay.transform.SetParent(CharacterParent, false);
                characterDisplay.index = character.index;
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
            _popupSelectedCharacter.SetActive(false);
            characterIndex.text = $"Character Index: {currentCharacter.index}";
        }
    }
}

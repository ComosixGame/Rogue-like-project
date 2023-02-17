using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
    // [SerializeField] private Text nameCharacter;
    // [SerializeField] private Text priceCharacter;
    [SerializeField] private Sprite thumb;

    [SerializeField] CharacterScripable characterScripable;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        //nameCharacter.text = $"Name character {characterScripable.characters[0].nameCharacter}";
        //priceCharacter.text = $"Price character {characterScripable.characters[0].priceCharacter}";
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = characterScripable.characters[0].thumb;
    }

    private void OnEnable() {
        gameManager.OnupdateInfoCharacter += updateInfoCharacter;
    }

    public void updateInfoCharacter(){
        //nameCharacter.text = $"Name character {characterScripable.characters[gameManager.characterSeleted].nameCharacter}";
        //priceCharacter.text = $"Price character {characterScripable.characters[gameManager.characterSeleted].priceCharacter}";
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = characterScripable.characters[gameManager.characterSeleted].thumb;
    }

    private void OnDisable() {
        gameManager.OnupdateInfoCharacter -= updateInfoCharacter;
    }
}

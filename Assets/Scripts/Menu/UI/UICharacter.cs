using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacter : MonoBehaviour
{
  
    [SerializeField] private Sprite thumb;

    [SerializeField] CharacterScripable characterScripable;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
      
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = characterScripable.characters[0].thumb;
    }

    private void OnEnable() {
        gameManager.OnupdateInfoCharacter += updateInfoCharacter;
    }

    public void updateInfoCharacter(){
        gameObject.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = characterScripable.characters[gameManager.characterSeleted].thumb;
    }

    private void OnDisable() {
        gameManager.OnupdateInfoCharacter -= updateInfoCharacter;
    }
}

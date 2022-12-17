using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject _chapter;
    public GameObject _shop;
    public GameObject _character;
    public GameObject _weapon;

    public GameObject _popupLoadComplete;
    public GameObject _popupMission;
    public GameObject _popupSettings;
    public GameObject _popupSelectedCharacter;
    public GameObject _popupConfirmWeapon;

    private void Start() {
        InitGame();
    }

    public void InitGame(){
        _chapter.SetActive(true);
        _shop.SetActive(false);
        _character.SetActive(false);
        _weapon.SetActive(false);
    }

    public void BtnChapter(){
        _chapter.SetActive(true);
        _shop.SetActive(false);
        _character.SetActive(false);
        _weapon.SetActive(false);
    } 

    public void BtnShop(){
        _chapter.SetActive(false);
        _shop.SetActive(true);
        _character.SetActive(false);
        _weapon.SetActive(false);
    }

    public void BtnCharacter(){
         _chapter.SetActive(false);
        _shop.SetActive(false);
        _character.SetActive(true);
        _weapon.SetActive(false);
    }

    public void BtnWeapon(){
        _chapter.SetActive(false);
        _shop.SetActive(false);
        _character.SetActive(false);
        _weapon.SetActive(true);
    }

    public void BtnLoadComplete(){
        _popupLoadComplete.SetActive(true);
        _popupMission.SetActive(false);
        _popupSettings.SetActive(false);
    }

    public void BtnMission(){
        _popupLoadComplete.SetActive(false);
        _popupMission.SetActive(true);
        _popupSettings.SetActive(false);
    }

    public void BtnSettings(){
        _popupLoadComplete.SetActive(false);
        _popupMission.SetActive(false);
        _popupSettings.SetActive(true);
    }
    public void BtnConfirmWeapon(){
        _popupConfirmWeapon.SetActive(true);
    }
    public void ClosePanelLoad(){
        _popupLoadComplete.SetActive(false);
    }

    public void ClosePanelMission(){
        _popupMission.SetActive(false);
    }

    public void ClosePanelSettings(){
        _popupSettings.SetActive(false);
    }

    public void ClosePanelSelectedCharacter(){
        _popupSelectedCharacter.SetActive(false);
    }

    public void ClosePanelSelectedWeapon(){
        _popupConfirmWeapon.SetActive(false);
    }

}
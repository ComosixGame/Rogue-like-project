using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public GameObject _popupSelectedChapter;
    public GameObject _popupConfirmNotEnoughMoney;
    public GameObject _popupConfirmSelected;
    [SerializeField] GunScriptable gunScriptable;
    [SerializeField] CharacterScripable characterScripable;
    [SerializeField] private CardGun gunPrefab;
    [SerializeField] List<CardGun> gunList = new List<CardGun>();
    [SerializeField] private Transform gunParent;
    Gun currentGun;
    [SerializeField] private GameObject _popUpSelectedCharacter;
    [SerializeField] private Text coins;
    [SerializeField] private Text nameCharacter;
    [SerializeField] private Text priceCharacter;
    [SerializeField] private Sprite thumb;
    private GameManager gameManager;
    private PlayerData playerData;
    private SoundManager soundManager;
    private void Awake() {
        gameManager = GameManager.Instance;
        playerData = PlayerData.Load();
        soundManager = SoundManager.Instance;
        coins.text = $"{playerData.coin}";
        // nameCharacter.text = $"Name character {characterScripable.characters[0].nameCharacter}";
        // priceCharacter.text = $"Price character {characterScripable.characters[0].priceCharacter}";
        Debug.Log(characterScripable.characters[0].thumb);
        //thumb = characterScripable.characters[0].thumb;
    }


    private void Start() {
        InitGame();

        for(int i = 0; i < gunScriptable.guns.Length; i++){
            CardGun gunDisplay = Instantiate(gunPrefab);
            gunList.Add(gunDisplay);
            gunDisplay.transform.SetParent(gunParent, false);
            gunDisplay.index = i;
            gunDisplay.priceGun = gunScriptable.guns[i].priceGun;
            gunDisplay.nameGun = gunScriptable.guns[i].nameGun;
            gunDisplay.thumb = gunScriptable.guns[i].thumb;
            //gunDisplay.RenderCard();
        }

    }

    private void OnEnable() {
        gameManager.OnUpdateCoinPlayer += updateCoinThenBuyItem;
        gameManager.OnNotEnoughMoney += showPopUpConfirmNotEnoughMoney;
        CardGun.ConfirmSelected += showPopUpConfirmSelected;
    }

    private void OnDisable() {
        gameManager.OnUpdateCoinPlayer -= updateCoinThenBuyItem;
        gameManager.OnNotEnoughMoney -= showPopUpConfirmNotEnoughMoney;
        CardGun.ConfirmSelected -= showPopUpConfirmSelected;
    }

    public void showPopUpConfirmNotEnoughMoney(){
        _popupConfirmNotEnoughMoney.SetActive(true);
    }

    public void showPopUpConfirmSelected(){
        _popupConfirmSelected.SetActive(true);
    }

    public void hidePopUpConfirmSelected(){
        _popupConfirmSelected.SetActive(false);
    }

    public void updateCoinThenBuyItem(int coin){
        coins.text = $"Coin: {coin}";
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

    public void btnOpenPopupSelectCharacter(){
        _popupSelectedCharacter.SetActive(true);
    }
    public void handleConfirmSelectedCharacter(){
        _popUpSelectedCharacter.SetActive(false);
    }
    
}
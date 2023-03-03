using System;
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
    public GameObject _popupSelectedChapter;
    public GameObject _popupConfirmNotEnoughMoney;
    public GameObject _popupConfirmSelected;
    public GameObject _popUpSelectedCharacter;
    [SerializeField] private Text coins;
    private GameManager gameManager;
    private PlayerData playerData;
    private SoundManager soundManager;
    private LoadSceneManager loadSceneManager;
    public static event Action OnReloadDailyMission;
    private void Awake() {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
    }


    private void Start() {
        InitGame();
        coins.text = $"{gameManager.coinPlayer}";
    }

    private void OnEnable() {
        gameManager.OnUpdateCoinPlayer += updateCoinThenBuyItem;
        gameManager.OnNotEnoughMoney += showPopUpConfirmNotEnoughMoney;
        CardGun.ConfirmSelected += showPopUpConfirmSelected;
        DailyMissionCard.OnCompletedMission += CompletedMission;
        gameManager.OnReceiveCoinReward += ReceiveCoinReward;
    }

    private void OnDisable() {
        gameManager.OnUpdateCoinPlayer -= updateCoinThenBuyItem;
        gameManager.OnNotEnoughMoney -= showPopUpConfirmNotEnoughMoney;
        CardGun.ConfirmSelected -= showPopUpConfirmSelected;
        DailyMissionCard.OnCompletedMission -= CompletedMission;
        gameManager.OnReceiveCoinReward -= ReceiveCoinReward;
    }

    public void CompletedMission(int goldReward){
        gameManager.IncreaseGoldReward(goldReward);
    }


    public void ReceiveCoinReward(int coin){
         coins.text = $"{coin}";
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
        coins.text = $"{coin}";
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
        OnReloadDailyMission?.Invoke();
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
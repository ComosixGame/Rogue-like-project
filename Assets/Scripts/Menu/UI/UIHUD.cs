using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using TMPro;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text gold;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private Transform NameModule;
    [SerializeField] private Transform NameModulePausePopup;
    [SerializeField] private UIModuleDisplay uiModuleDisplay;
    [SerializeField] private GameObject _PopupPause;
    [SerializeField] private GameObject _PopupModuleSelector;
    [SerializeField] private GameObject _PopupSettings;
    [SerializeField] private GameObject _PopupLobby;
    [SerializeField] private GameObject _FooterLevel;
    [SerializeField] private GameObject _PopupLoseGame;
    [SerializeField] private GameObject _PopupWinGame;
    [SerializeField] private GameObject _Pause;
    [SerializeField] private GameObject _Stage;
    [SerializeField] private GameObject _LabUI;
    [SerializeField] private GameObject _postionValueModule;
    [SerializeField] private GameObject _postionValueModulePausePopup;
    [SerializeField] private Button _btnGetMore;
    [SerializeField] private SelectAbilityButton abilityBtn;
    [SerializeField] private Transform abilitySelectorContainer;
    [SerializeField] private Text abilityEmptyMessage;
    [SerializeField] private TMP_Text goldEndGame;
    
    //singleton
    private GameManager gameManager;
    private AbilityModuleManager abilityModuleManager;
    private LoadSceneManager loadSceneManager;
    private PlayerDamageble playerDamageble;
    private ObjectPoolerManager objectPoolerManager;
    private PlayerData playerData;
    private DailyMissionManager dailyMissionManager;
    private SoundManager soundManager;

    //event

    //sound
    public AudioClip soundBasic;
    public AudioClip btnSound;

    private void Awake() {
        playerData = PlayerData.Load();
        gameManager = GameManager.Instance;
        abilityModuleManager = AbilityModuleManager.Instance;
        objectPoolerManager = ObjectPoolerManager.Instance;
        loadSceneManager = LoadSceneManager.Instance;
        soundManager = SoundManager.Instance;
    }

    private void OnEnable() {
        MapGeneration.OnLevelChange += ChangeLevelText;
        MapGeneration.OnWaveChange += ChangeWavelText;
        MapGeneration.OnWinGame += ChangeWinGame;
        abilityModuleManager.OnShowAbilityModuleSeletion += showAbilityModuleSeletion;
        abilityModuleManager.OnAddAbility += HandleAddAbility;
        abilityModuleManager.OnAddAbility += HandleAddModulePausePopup;
        gameManager.OnUpdateCoin += ChangeUpdateCoins;
        PlayerDamageble.OnLoseGame += OnLoseGame;
        door.OnOpenDoor += LoadedWinGame;
    }

    private void Start() {
        soundManager.SetMusicBackGround(soundBasic);
        soundManager.SetPlayMusic(true);
    }

    private void OnDisable() {
        MapGeneration.OnLevelChange -= ChangeLevelText;
        MapGeneration.OnWaveChange -= ChangeWavelText;
        MapGeneration.OnWinGame -= ChangeWinGame;
        abilityModuleManager.OnShowAbilityModuleSeletion -= showAbilityModuleSeletion;
        abilityModuleManager.OnAddAbility -= HandleAddAbility;
        abilityModuleManager.OnAddAbility -= HandleAddModulePausePopup;
        gameManager.OnUpdateCoin -= ChangeUpdateCoins;
        PlayerDamageble.OnLoseGame -= OnLoseGame;
        door.OnOpenDoor -= LoadedWinGame;
    }


    public void InitScene(){
        _PopupLobby.SetActive(false);
        _PopupPause.SetActive(false);
        _PopupModuleSelector.SetActive(false);
        _PopupSettings.SetActive(false);
        _PopupLoseGame.SetActive(false);
        _PopupWinGame.SetActive(false);
        _Pause.SetActive(true);
        _Stage.SetActive(true);
        _LabUI.SetActive(true);
        _FooterLevel.SetActive(true);
    }

    public void HandlePause(){
        _FooterLevel.SetActive(false);
        _Stage.SetActive(false);
        _LabUI.SetActive(false);
        _Pause.SetActive(false);
        _PopupPause.SetActive(true);
        _postionValueModulePausePopup.GetComponent<Scrollbar>().value = 0;
        gameManager.PauseGame();
    }

    public void HandleSettings(){
        _PopupSettings.SetActive(true);
    }

    public void HandleOpenLobby(){
        _PopupLobby.SetActive(true);
    }

    public void HandleResume(){
        InitScene();
        abilityModuleManager.ResetAbility();
        gameManager.ResumeGame();
    }

    public void HandleCloseSetting(){
        _PopupSettings.SetActive(false);
    }

    public void HandleClosePopupLobby(){
        _PopupLobby.SetActive(false);
    }

    public void OnLoadScene(string path){
        loadSceneManager.LoadScene(path);
    }

    public void ChangeWinGame(){
        _PopupWinGame.SetActive(true);
        gameManager.EndGame(true);
    }

    private void ChangeLevelText(int currentLevel) {
        levelText.text = $"LAB : {currentLevel}";
        stateText.text = $"State: {currentLevel}";
    }

    private void ChangeWavelText(int currentWave) {
        waveText.text = $"WARE {currentWave}/{gameManager.waves}";
    }

    private void showAbilityModuleSeletion(int numberAbilityAvaiable){
        if(numberAbilityAvaiable == 0) {
            abilityEmptyMessage.gameObject.SetActive(true);
        }
        abilityModuleManager.RenderAbilitySelector(abilitySelectorContainer, abilityBtn, 3);
        

        _PopupModuleSelector.SetActive(true);
        _postionValueModule.GetComponent<Scrollbar>().value = 0;
        _btnGetMore.interactable = true;
    }
     
    public void ChangeUpdateCoins(int currentCoin){
        gold.text = $"{currentCoin}";
        goldEndGame.text = $"{currentCoin}";
    }



    public void HandleAddAbility(AbsAbilityModule newAbility){
        abilityEmptyMessage.gameObject.SetActive(false);
        _PopupModuleSelector.SetActive(false);
        gameManager.ResumeGame();
        var itemUIClone = Instantiate(uiModuleDisplay, Vector3.zero, Quaternion.identity);
        itemUIClone.transform.SetParent(NameModule, false);
        itemUIClone.GetComponent<UIModuleDisplay>().SetText(newAbility.abilityName);
    }

    public void HandleAddModulePausePopup(AbsAbilityModule newAbility){
        var itemUIClone = Instantiate(uiModuleDisplay, Vector3.zero, Quaternion.identity);
        itemUIClone.transform.SetParent(NameModulePausePopup, false);
        itemUIClone.GetComponent<UIModuleDisplay>().SetText(newAbility.abilityName);
    }
    public void HandleGetMoreOptions(){
        _btnGetMore.interactable = false;
    }

    public void OnLoseGame(){
        _PopupLoseGame.SetActive(true);
    }


    public void HandleContinueGame(){
        InitScene();
        loadSceneManager.ResetScene();
    }

    public void HandleExitGame(string path){
        OnLoadScene(path);
    }

    public void PlayBtnSound(){
        soundManager.PlaySound(btnSound);
    }

    public void LoadedWinGame(){
        _PopupWinGame.SetActive(true);
        gameManager.EndGame(true);
    }
}

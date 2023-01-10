using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private Text levelText;
    [SerializeField] private Text waveText;
    [SerializeField] private Text gold;

    [SerializeField] private Transform NameModule;
    [SerializeField] private UIModuleDisplay uiModuleDisplay;

    [SerializeField] private GameObject _PopupPause;
    [SerializeField] private GameObject _PopupModuleSelector;
    [SerializeField] private GameObject _PopupSettings;
    [SerializeField] private GameObject _PopupLobby;
    [SerializeField] private GameObject _FooterLevel;
    [SerializeField] private GameObject _Pause;
    [SerializeField] private GameObject _Stage;
    [SerializeField] private GameObject _LabUI;
    [SerializeField] private GameObject _optionModule1;
    [SerializeField] private GameObject _postionValueModule;
    [SerializeField] private Button _btnGetMore;
    [SerializeField] private SelectAbilityButton abilityBtn;
    [SerializeField] private Transform abilitySelectorContainer;
    [SerializeField] private Text abilityEmptyMessage;
    private GameManager gameManager;

    private AbilityModuleManager abilityModuleManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        abilityModuleManager = AbilityModuleManager.Instance;
    }

    private void OnEnable() {
        MapGeneration.OnLevelChange += ChangeLevelText;
        MapGeneration.OnWaveChange += ChangeWavelText;
        abilityModuleManager.OnShowAbilityModuleSeletion += showAbilityModuleSeletion;
        abilityModuleManager.OnAddAbility += HandleAddAbility;
        gameManager.OnUpdateCoin += ChangeUpdateCoins;
    }

    private void OnDisable() {
        MapGeneration.OnLevelChange -= ChangeLevelText;
        MapGeneration.OnWaveChange -= ChangeWavelText;
        abilityModuleManager.OnShowAbilityModuleSeletion -= showAbilityModuleSeletion;
        abilityModuleManager.OnAddAbility -= HandleAddAbility;
        gameManager.OnUpdateCoin -= ChangeUpdateCoins;
    }

    public void InitScene(){
        _PopupLobby.SetActive(false);
        _PopupPause.SetActive(false);
        _PopupModuleSelector.SetActive(false);
        _PopupSettings.SetActive(false);
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
        gameManager.ResumeGame();
    }

    public void HandleCloseSetting(){
        _PopupSettings.SetActive(false);
    }

    public void HandleClosePopupLobby(){
        _PopupLobby.SetActive(false);
    }

    public void HandleReturnLobby(){
        SceneManager.LoadScene("Scenes/UI");
    }

    private void ChangeLevelText(int currentLevel) {
        levelText.text = $"Lab : {currentLevel}";
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
     
    private void ChangeUpdateCoins(int currentCoin){
        gold.text = $"{currentCoin}";
    }

    public void HandleAddAbility(AbsAbilityModule newAbility){
        abilityEmptyMessage.gameObject.SetActive(false);
        _PopupModuleSelector.SetActive(false);
        gameManager.ResumeGame();
        var itemUIClone = Instantiate(uiModuleDisplay, Vector3.zero, Quaternion.identity);
        itemUIClone.transform.SetParent(NameModule, false);
        itemUIClone.GetComponent<UIModuleDisplay>().SetText(newAbility.abilityName);
    }

    public void HandleAddModule(AbsAbilityModule newAbility){
        
    }


    public void HandleGetMoreOptions(){
        _optionModule1.SetActive(true);
        _btnGetMore.interactable = false;
    }

    private void UpdateModule(){

    }
}

using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardGun : MonoBehaviour
{
    CardGun currentGun = null;
    public int index;
    public string nameGun;
    public int priceGun;
    public Sprite thumb;
    [SerializeField] private TMP_Text nameGunText;
    [SerializeField] private GameObject _btnBuy;
    [SerializeField] private GameObject _btnSelected;
    [SerializeField] private TMP_Text _ownerGun;
    private bool own;

    //singleton
    private GameManager gameManager;
    private SoundManager soundManager;

    //Action
    public static event Action ConfirmSelected;
    
    //sound
    public AudioClip btnSound;

    private void Awake() {
        gameManager = GameManager.Instance;
        currentGun =  GetComponent<CardGun>();
        soundManager = SoundManager.Instance;
    }

    private void OnEnable() {
        gameManager.OnupdateStatus += HandleUpdateStatus;
    }

    private void Start() {
        nameGunText.text = $"{nameGun}";
        gameObject.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = currentGun.thumb;

        own = gameManager.weaponOwn.IndexOf(currentGun.index) != -1;

        if(own){
            _ownerGun.text = $"Owner";
        }else{
            _ownerGun.text = $"{priceGun}$";
        }
        _btnBuy.SetActive(!own);
        _btnSelected.SetActive(own);

    }

    public void HandleUpdateStatus(){
        _ownerGun.text = $"Owner";
    }

    public void SelectedWeapon(){
        gameManager.SelectedWeapon(currentGun.index);
        ConfirmSelected?.Invoke();
    }

    public void BuyWeapon(){
        bool success = gameManager.BuyWeapon(currentGun.index, currentGun.priceGun);
        _btnBuy.SetActive(!success);
        _btnSelected.SetActive(success);
    }

    public void RenderCard(Sprite thumb, string name) {
        
    }

    private void OnDisable() {
        gameManager.OnupdateStatus -= HandleUpdateStatus;
    }

    public void PlayBtnSound(){
        soundManager.PlaySound(btnSound);
    }
}

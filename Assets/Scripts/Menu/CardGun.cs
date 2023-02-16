using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGun : MonoBehaviour
{
    public int index;
    public string nameGun;
    public int priceGun;
    public Sprite thumb;

    CardGun currentGun = null;

    [SerializeField] private Text nameGunText;
    [SerializeField] private GameObject _btnBuy;
    [SerializeField] private GameObject _btnSelected;
    [SerializeField] private Text _ownerGun;

    private bool own;
    private GameManager gameManager;

    public static event Action ConfirmSelected;
    
    private void Awake() {
        gameManager = GameManager.Instance;
        currentGun =  GetComponent<CardGun>();
    }

    private void OnEnable() {
        gameManager.OnupdateStatus += HandleUpdateStatus;
    }

    private void Start() {
        nameGunText.text = $"{nameGun}";
        gameObject.transform.GetChild(1).GetComponent<Image>().sprite = currentGun.thumb;

        own = gameManager.weaponOwn.IndexOf(currentGun.index) != -1;

        if(own){
            _ownerGun.text = $"Owner";
        }else{
            _ownerGun.text = $"{priceGun}";
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
}

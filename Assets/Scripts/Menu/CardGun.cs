using System;
using UnityEngine;
using UnityEngine.UI;

public class CardGun : MonoBehaviour
{
    public int index;
    public string nameGun;
    public int priceGun;

    CardGun currentGun = null;

    [SerializeField] private Text nameGunText;
    [SerializeField] private GameObject _btnBuy;
    [SerializeField] private GameObject _btnSelected;

    private GameManager gameManager;

    public static event Action OnNotEnoughMoney;
    
    private void Awake() {
        gameManager = GameManager.Instance;
        currentGun =  GetComponent<CardGun>();
    }

    private void Start() {
        nameGunText.text = $"{nameGun}";

        bool own = gameManager.weaponOwn.IndexOf(currentGun.index) != -1;

        _btnBuy.SetActive(!own);
        _btnSelected.SetActive(own);

    }

     public void SelectedWeapon(){
        gameManager.SelectedWeapon(currentGun.index);
    }

    public void BuyWeapon(){
        bool success = gameManager.BuyWeapon(currentGun.index, currentGun.priceGun);
        OnNotEnoughMoney?.Invoke();
        _btnBuy.SetActive(!success);
        _btnSelected.SetActive(success);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public int index;
    public string nameGun;
    public int priceGun;

    Gun currentGun = null;

    [SerializeField] private Text nameGunText;
    [SerializeField] GameObject _btnBuy;
    [SerializeField] GameObject _btnSelected;

    private GameManager gameManager;
    
    private void Awake() {
        gameManager = GameManager.Instance;
        currentGun =  GetComponent<Gun>();
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
        _btnBuy.SetActive(false);
        _btnSelected.SetActive(true);
    }
}

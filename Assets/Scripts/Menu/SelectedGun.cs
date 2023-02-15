using UnityEngine;

public class SelectedGun : MonoBehaviour
{
    [SerializeField] GameObject _btnBuy;
    [SerializeField] GameObject _btnSelect;
    Gun currentGun = null;
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameManager.Instance;
        currentGun =  GetComponent<Gun>();
    }

    public void SelectedWeapon(){
        gameManager.SelectedWeapon(currentGun.index);
    }

    public void BuyWeapon(){
        bool success = gameManager.BuyWeapon(currentGun.index, currentGun.priceGun);
        _btnBuy.SetActive(false);
        _btnSelect.SetActive(true);
    }
}

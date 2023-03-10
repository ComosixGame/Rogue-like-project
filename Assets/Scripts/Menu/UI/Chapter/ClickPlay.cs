using UnityEngine;
using UnityEngine.UI;

public class ClickPlay : MonoBehaviour
{
    private EnergyManager energyManager;
    [SerializeField] private int energy;
    private void Awake() {
        energyManager = EnergyManager.Instance;
    }

    private void OnEnable() {
        GetComponent<Button>().onClick.AddListener(Click);
    }

    public void Click() {
        energyManager.SpendEnergy(energy);
    }

    private void OnDisable() {
        GetComponent<Button>().onClick.RemoveListener(Click);
    }
}

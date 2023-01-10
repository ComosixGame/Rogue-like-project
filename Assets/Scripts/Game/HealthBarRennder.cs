using UnityEngine;
using UnityEngine.UI;

public class HealthBarRennder : MonoBehaviour
{
    public GameObject healthBar;
    public float offset;
    private Camera _camera;
    private GameObject _heathBar;
    private Slider sliderHealthBar;

    public void CreateHealthBar(Transform parent, float Maxhealth){
        _camera = Camera.main;
        _heathBar = GameObject.Instantiate(healthBar);
        _heathBar.transform.SetParent(parent, false);
        _heathBar.transform.position = parent.position + Vector3.up * offset;
        sliderHealthBar = _heathBar.GetComponentInChildren<Slider>();
        sliderHealthBar.maxValue = Maxhealth;
        sliderHealthBar.value = Maxhealth;
    }

    public void UpdateHealthBarRotation(){
        Vector3 dirCam = _camera.transform.position - _heathBar.transform.position;
        dirCam.x = 0;
        _heathBar.transform.rotation = Quaternion.LookRotation(dirCam.normalized);
    }

    public void UpdateHealthBarValue(float health){
        sliderHealthBar.value = health;
    }
}

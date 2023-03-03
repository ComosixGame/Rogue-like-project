using System.Collections.Generic;
using UnityEngine;

public class SelectedGun : MonoBehaviour
{
    [SerializeField] GunScriptable gunScriptable;
    [SerializeField] Transform gunParent;
    [SerializeField] private CardGun gunPrefab;
    [SerializeField] List<CardGun> gunList = new List<CardGun>();
    //Gun currentGun;

    private void Start() {
        for (int i = 0; i < gunScriptable.guns.Length; i++)
        {
            CardGun gunDisplay = Instantiate(gunPrefab);
            gunList.Add(gunDisplay);
            gunDisplay.transform.SetParent(gunParent, false);
            gunDisplay.index = i;
            gunDisplay.priceGun = gunScriptable.guns[i].priceGun;
            gunDisplay.nameGun = gunScriptable.guns[i].nameGun;
            gunDisplay.thumb = gunScriptable.guns[i].thumb;
        }
    }
}

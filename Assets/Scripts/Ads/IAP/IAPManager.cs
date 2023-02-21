using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour
{
    private string coin500 = "com.tab.roguelike.coin500";
    private string coin1000 = "com.tab.roguelike.coin1000";

    public void OnPurchaseComplete(Product product){
       if(product.definition.id == coin500){
            //reward your players
            Debug.Log("You've gained 500 coins");
       }

       if(product.definition.id == coin1000){
            //reward your player
            Debug.Log("You've gained 1000 coins");
       }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason){
        Debug.Log(product.definition.id + " failed because" + failureReason);
    }


}

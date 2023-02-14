using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeShopUI : MonoBehaviour
{
    public Transform shopItemTemplate;

    public CoinPicker cp;

    private void Awake() {
    }

    private void Start() {
       // CreateItemButton("Damage", 3);

    }

    // private void CreateItemButton(string itemName, int itemCost) {
    //     Transform shopItemTransform = Instantiate(shopItemTemplate, container);
    //     shopItemTransform.gameObject.SetActive(true);

    //     RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

    //     shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
    //     shopItemTransform.Find("priceText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
        
    // }

    public void BuyItem() {
    
        if (cp.coin >= 3) {
            Debug.Log("item bought");
            cp.coin-=3;
            cp.textCoins.SetText("Credits: " + (cp.coin));
        }
    }

    public void CloseShop() {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}

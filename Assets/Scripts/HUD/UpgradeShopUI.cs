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

    public void BuyDamage() {
        if (cp.getCoin() >= 50) {
            Debug.Log("damage bought");
            cp.withdraw(50);
        }
    }


    public void BuySpeed() {
        if (cp.getCoin() >= 50) {
            Debug.Log("speed bought");
            cp.withdraw(50);
        }
    }

    public void BuyDash() {
        if (cp.getCoin() >= 100) {
            Debug.Log("dash bought");
            cp.withdraw(100);
        }
    }

    public void BuyJump() {
        if (cp.getCoin() >= 200) {
            Debug.Log("double jump bought");
            cp.withdraw(200);
        }
    }
    public void CloseShop() {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
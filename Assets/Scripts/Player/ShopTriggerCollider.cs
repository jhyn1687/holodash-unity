using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShopTriggerCollider : MonoBehaviour
{
    [SerializeField] private UpgradeShopUI uiShop;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "UpgradeBox") {
            Debug.Log("sphere collision");
            uiShop.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        uiShop.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}

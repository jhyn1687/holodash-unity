using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UpgradeShopUI : MonoBehaviour
{
    public static event Action OnBuyDamage;

    public static event Action OnBuySpeed;

    public static event Action OnBuyHP;

    public static event Action OnBuyJump;

    public Transform shopItemTemplate;

    public CoinPicker cp;

    [Header("Damage")]
    public RectTransform damageButton;
    public int damageInitialPrice;
    public int damagePriceIncrease;
    public int damageMaxPurchases;

    [Header("Speed")]
    public RectTransform speedButton;
    public int speedInitialPrice;
    public int speedPriceIncrease;
    public int speedMaxPurchases;

    [Header("HP")]
    public RectTransform hpButton;
    public int hpInitialPrice;
    public int hpPriceIncrease;
    public int hpMaxPurchases;

    [Header("Jump")]
    public RectTransform jumpButton;
    public int jumpInitialPrice;
    public int jumpPriceIncrease;
    public int jumpMaxPurchases;

    private int damagePurchases, speedPurchases, hpPurchases, jumpPurchases;
    private void Awake() {
    }

    private void Update() {
        damagePurchases = GameManager.Instance.damageUpgrade;
        speedPurchases = GameManager.Instance.speedUpgrade;
        hpPurchases = GameManager.Instance.hpUpgrade;
        jumpPurchases = GameManager.Instance.jumpUpgrade;

        int damageCurrentPrice = damageInitialPrice + damagePriceIncrease * damagePurchases;
        int speedCurrentPrice = speedInitialPrice + speedPriceIncrease * speedPurchases;
        int hpCurrentPrice = hpInitialPrice + hpPriceIncrease * hpPurchases;
        int jumpCurrentPrice = jumpInitialPrice + jumpPriceIncrease * jumpPurchases;

        speedButton.GetComponentInChildren<TextMeshProUGUI>().text = speedCurrentPrice.ToString();
        hpButton.GetComponentInChildren<TextMeshProUGUI>().text = hpCurrentPrice.ToString();
        jumpButton.GetComponentInChildren<TextMeshProUGUI>().text = jumpCurrentPrice.ToString();

        damageButton.Find("Slider").GetComponent<Image>().fillAmount = (float)damagePurchases / damageMaxPurchases;
        speedButton.Find("Slider").GetComponent<Image>().fillAmount = (float)speedPurchases / speedMaxPurchases;
        hpButton.Find("Slider").GetComponent<Image>().fillAmount = (float)hpPurchases / hpMaxPurchases;
        jumpButton.Find("Slider").GetComponent<Image>().fillAmount = (float)jumpPurchases / jumpMaxPurchases;
        
        if (damagePurchases >= damageMaxPurchases) {
            damageButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            damageButton.Find("Coin").GetComponent<Image>().enabled = false;
            damageButton.GetComponent<Button>().interactable = false;
        } else {
            damageButton.GetComponentInChildren<TextMeshProUGUI>().text = damageCurrentPrice.ToString();
            if (cp.getCoin() < damageCurrentPrice) {
                damageButton.GetComponent<Button>().interactable = false;
            } else {
                damageButton.GetComponent<Button>().interactable = true;
            }
        }

        if (speedPurchases >= speedMaxPurchases) {
            speedButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            speedButton.Find("Coin").GetComponent<Image>().enabled = false;
            speedButton.GetComponent<Button>().interactable = false;
        } else {
            speedButton.GetComponentInChildren<TextMeshProUGUI>().text = speedCurrentPrice.ToString();
            if (cp.getCoin() < speedCurrentPrice) {
                speedButton.GetComponent<Button>().interactable = false;
            } else {
                speedButton.GetComponent<Button>().interactable = true;
            }
        }

        if (hpPurchases >= hpMaxPurchases) {
            hpButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            hpButton.Find("Coin").GetComponent<Image>().enabled = false;
            hpButton.GetComponent<Button>().interactable = false;
        } else {
            hpButton.GetComponentInChildren<TextMeshProUGUI>().text = hpCurrentPrice.ToString();
            if (cp.getCoin() < hpCurrentPrice) {
                hpButton.GetComponent<Button>().interactable = false;
            } else {
                hpButton.GetComponent<Button>().interactable = true;
            }
        }

        if (jumpPurchases >= jumpMaxPurchases) {
            jumpButton.GetComponentInChildren<TextMeshProUGUI>().text = "MAX";
            jumpButton.Find("Coin").GetComponent<Image>().enabled = false;
            jumpButton.GetComponent<Button>().interactable = false;
        } else {
            jumpButton.GetComponentInChildren<TextMeshProUGUI>().text = jumpCurrentPrice.ToString();
            if (cp.getCoin() < jumpCurrentPrice) {
                jumpButton.GetComponent<Button>().interactable = false;
            } else {
                jumpButton.GetComponent<Button>().interactable = true;
            }
        }


    }

    public void BuyDamage() {
        Debug.Log("damage bought");
        OnBuyDamage?.Invoke();
        cp.withdraw(damageInitialPrice + damagePriceIncrease * damagePurchases);
    }

    public void BuySpeed() {
        Debug.Log("speed bought");
        OnBuySpeed?.Invoke();
        cp.withdraw(speedInitialPrice + speedPriceIncrease * speedPurchases);
    }

    public void BuyHP() {
        Debug.Log("hp bought");
        OnBuyHP?.Invoke();
        cp.withdraw(hpInitialPrice + hpPriceIncrease * hpPurchases);
    }

    public void BuyJump() {
        Debug.Log("double jump bought");
        OnBuyJump?.Invoke();
        cp.withdraw(jumpInitialPrice + jumpPriceIncrease * jumpPurchases);
    }
    
    public void CloseShop() {
        this.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
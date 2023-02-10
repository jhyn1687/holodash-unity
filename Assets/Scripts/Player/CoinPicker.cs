using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinPicker : MonoBehaviour, IDataPersistence
{
    private float coin = 0;

    private SpriteRenderer visual;

    public static event Action OnCoinCollected;

    private void Awake() {
        visual = this.GetComponent<SpriteRenderer>();
    }

    public void LoadData(GameData data) 
    {
        coin = data.coinsCollected;
    }

    public void SaveData(GameData data) 
    {
        data.coinsCollected = coin;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Coin") {
            coin++;
            Destroy(other.gameObject);
            GameManager.Instance.CoinCollected();
        }
    }
}

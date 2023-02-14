using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinPicker : MonoBehaviour, IDataPersistence
{
    private float coin = 0;
    
    public static event Action OnCoinCollected;
    
    public void LoadData(GameData data) {
        coin = data.coinsCollected;
    }

    public void SaveData(GameData data) {
        data.coinsCollected = coin;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Coin") {
            coin++;
            OnCoinCollected?.Invoke();
            Destroy(other.gameObject);
        }
    }
}

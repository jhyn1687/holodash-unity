using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinPicker : MonoBehaviour, IDataPersistence {
    private int coin = 0;

    public static event Action<int> OnCoinChange;

    public void LoadData(GameData data) {
        coin = data.coinsCollected;
    }

    public void SaveData(GameData data) {
        data.coinsCollected = coin;
    }
    public void deposit(int amount) {
        coin += amount;
        OnCoinChange?.Invoke(coin);
    }
    public void withdraw(int amount) {
        coin -= amount;
        OnCoinChange?.Invoke(coin);
    }
    public int getCoin() {
        return coin;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Coin") {
            CoinScript coinScript;
            other.TryGetComponent<CoinScript>(out coinScript);
            if (coinScript != null && coinScript.tryCollect()) {
                deposit(coinScript.value());
            }
            Destroy(other.gameObject);
        }
    }
}
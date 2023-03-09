using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CoinPicker : MonoBehaviour, IDataPersistence
{
    private int coin = 0;
    // private bool canCollect = true;

    public static event Action<int> OnCoinChange;
    
    public void LoadData(GameData data) {
        coin = data.coinsCollected;
    }

    public void SaveData(GameData data) {
        data.coinsCollected = coin;
    }

    public void withdraw(int amount) {
        coin -= amount;
        OnCoinChange?.Invoke(coin);
    }

    public int getCoin() {
        return coin;
    }

    // enables or disables the collection of coins.
    // true = can collect coins
    // false = can't 
    // public void SetCollectionState(bool state) {
    //     if (state) 
    //     {
    //         canCollect = true;
    //     } 
    //     else 
    //     {
    //         canCollect = false;
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Coin") {
            coin++;
            OnCoinChange?.Invoke(coin);
            Destroy(other.gameObject);
        }
    }
}

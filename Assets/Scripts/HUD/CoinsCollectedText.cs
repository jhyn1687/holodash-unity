using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCollectedText : MonoBehaviour, IDataPersistence
{
    private float coinsCollected;

    private TextMeshProUGUI coinsCollectedText;

    private void Awake() 
    {
        coinsCollectedText = this.GetComponent<TextMeshProUGUI>();
    }
    
    public void LoadData(GameData data) 
    {
        coinsCollected = data.coinsCollected;
        coinsCollectedText.text = "Credits: " + coinsCollected;
    }

    public void SaveData(GameData data)
    {
        // no data needs to be saved for this script
    }

    private void OnCoinChange(int newBalance) 
    {
        coinsCollected = newBalance;
        coinsCollectedText.text = "Credits: " + coinsCollected;
    }

    private void Update() 
    {
        coinsCollectedText.text = "Credits: " + coinsCollected;
    }
    
    private void OnEnable() {
        CoinPicker.OnCoinChange += OnCoinChange;
    }
    private void OnDisable() {
        CoinPicker.OnCoinChange -= OnCoinChange;
    }
}
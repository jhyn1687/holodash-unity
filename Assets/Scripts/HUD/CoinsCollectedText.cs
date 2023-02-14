using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsCollectedText : MonoBehaviour, IDataPersistence
{
    private float coinsCollected = 0;

    private TextMeshProUGUI coinsCollectedText;

    private void Awake() 
    {
        coinsCollectedText = this.GetComponent<TextMeshProUGUI>();
    }
    
    public void LoadData(GameData data) 
    {
        coinsCollected = data.coinsCollected;
    }

    public void SaveData(GameData data)
    {
        // no data needs to be saved for this script
    }

    private void OnCoinCollected() 
    {
        coinsCollected++;
    }

    private void Update() 
    {
        coinsCollectedText.text = "Credits: " + coinsCollected;
    }
    
    private void OnEnable() {
        CoinPicker.OnCoinCollected += OnCoinCollected;
    }
    private void OnDisable() {
        CoinPicker.OnCoinCollected -= OnCoinCollected;
    }
}
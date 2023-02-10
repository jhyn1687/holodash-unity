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

    private void Start() 
    {
        // subscribe to events
        GameManager.Instance.onCoinCollected += OnCoinCollected;
    }

    public void LoadData(GameData data) 
    {
        coinsCollected = data.coinsCollected;
    }

    public void SaveData(GameData data)
    {
        // no data needs to be saved for this script
    }

    private void OnDestroy() 
    {
        // unsubscribe from events
        GameManager.Instance.onCoinCollected -= OnCoinCollected;
    }

    private void OnCoinCollected() 
    {
        coinsCollected++;
    }

    private void Update() 
    {
        coinsCollectedText.text = "" + coinsCollected;
    }
}
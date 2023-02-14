using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinPicker : MonoBehaviour
{
    public float coin = 0;

    [SerializeField] public TextMeshProUGUI textCoins;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Coin") {
            coin++;
            textCoins.SetText("Credits: " + coin);
            Destroy(other.gameObject);
        }
    }
}

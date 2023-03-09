using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour {
    [SerializeField] private int coinValue = 1;
    private bool isCollected = false;
    public bool tryCollect() {
        if (!isCollected) {
            isCollected = true;
            return true;
        }
        return !isCollected;
    }

    public int value() {
        return coinValue;
    }
}
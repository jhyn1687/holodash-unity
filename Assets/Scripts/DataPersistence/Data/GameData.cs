using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coinsCollected;
    public int damageUpgrade;
    public int speedUpgrade;
    public int hpUpgrade;
    public int jumpUpgrade;
    public bool tutorialFinished;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        coinsCollected = 0;
        damageUpgrade = 0;
        speedUpgrade = 0;
        hpUpgrade = 0;
        jumpUpgrade = 0;
        tutorialFinished = false;
    }
}
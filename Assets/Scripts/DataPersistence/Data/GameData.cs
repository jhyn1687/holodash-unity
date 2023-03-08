using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int coinsCollected;
    public Upgrades upgrades;

    public TutorialData tutorialdata;

    // the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData() 
    {
        coinsCollected = 0;
        upgrades = new Upgrades();
        tutorialdata = new TutorialData();
    }
}
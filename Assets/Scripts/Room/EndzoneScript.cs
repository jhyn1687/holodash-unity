using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndzoneScript : MonoBehaviour, IDataPersistence
{
    public static event Action EndzoneReached;

    private bool endReached = false;

    public void LoadData(GameData data)
    {
        endReached = data.tutorialFinished;
        Debug.Log(endReached);
    }

    public void SaveData(GameData data)
    {
        data.tutorialFinished = endReached;
        Debug.Log(endReached);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            EndzoneReached?.Invoke();
            endReached = true;
            Debug.Log(endReached);
            GameObject.Destroy(gameObject);
        }
    }
}

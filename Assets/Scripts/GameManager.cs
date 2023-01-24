using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                Debug.Log("Game Manager is null");
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
    }

}

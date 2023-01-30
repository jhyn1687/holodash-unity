using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] public int CHAPTER;

    private static GameManager _instance;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                Debug.Log("Game Manager is null");
            }
            return _instance;
        }
    }

    private ChapterManager chapterScript;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        chapterScript = GetComponent<ChapterManager>();
        InitGame();
    }


    void InitGame()
    {
        chapterScript.initChapter(CHAPTER);

    }

}

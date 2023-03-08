using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static event Action OnReset;
    [SerializeField] private int startingChapter;
    [SerializeField] private TextMeshProUGUI AugmentPickupText;

    public int currentChapter;

    private Transform enemyBulletContainer;

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

    void Start() 
    {
        if (DataPersistenceManager.gameManager.tutorialFinished) {
            ChapterManager.Instance.InitGame();
        } else {
            ChapterManager.Instance.InitFirstTimePlay();
        }
    }

    private void OnEndChapterZoneReached() 
    {
        Dictionary<string, object> chapterData = new Dictionary<string, object>(){
            {"chapter", currentChapter },
            {"timeSinceStartup", Time.realtimeSinceStartup }
        };
        AnalyticsService.Instance.CustomData("chapterEndReached", chapterData);
        currentChapter += 1;
        LoadNewChapter(currentChapter);
    }

    private void Reset() 
    {
        currentChapter = startingChapter;
        LoadNewChapter(currentChapter);
        OnReset?.Invoke();
    }

    private void LoadNewChapter(int currChapter)
    {
        ChapterManager.Instance.initChapter(currChapter);
        ClearLevel();
    }


    public void OnAugmentPickup(int id) 
    {
        Augment aug = AugmentManager.GetAugment(id);
        StartCoroutine(AugmentPickup(aug));
    }

    IEnumerator AugmentPickup(Augment aug)
    {
        yield return new WaitUntil(() => !AugmentPickupText.IsActive());
        AugmentPickupText.SetText(aug.name + "\n" + aug.description);
        AugmentPickupText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        AugmentPickupText.gameObject.SetActive(false);
    }

    private void ClearLevel()
    {
        if (enemyBulletContainer != null)
        {
            foreach (Transform child in enemyBulletContainer)
            {
                Destroy(child.gameObject);
            }
        }
        else if (GameObject.Find("Enemy Bullet Container") != null)
        {
            enemyBulletContainer = GameObject.Find("Enemy Bullet Container").transform;
        }
    }

    private void OnEnable() {
        EndChapterScript.EndChapterZoneReached += OnEndChapterZoneReached;
        PlayerBehavior.OnPlayerDeath += Reset;
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
    }

    private void OnDisable() {
        EndChapterScript.EndChapterZoneReached -= OnEndChapterZoneReached;
        PlayerBehavior.OnPlayerDeath -= Reset;
        AugmentManager.OnAugmentPickup -= OnAugmentPickup;
    }
}

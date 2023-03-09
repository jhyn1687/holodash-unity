using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static event Action OnReset;
    public static event Action OnDataLoaded;
    [SerializeField] private int startingChapter;
    [SerializeField] private TextMeshProUGUI AugmentPickupText;

    public int currentChapter;

    private Transform enemyBulletContainer;

    public int damageUpgrade;
    public int speedUpgrade;
    public int hpUpgrade;
    public int jumpUpgrade;

    public bool tutorialFinished;

    public GameObject Loading;

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
        GameObject HUD = GameObject.Find("HUD");
        if (HUD == null) {
            Debug.LogError("HUD not found");
        }
        Loading = HUD.transform.Find("Loading").gameObject;
        if (Loading == null) {
            Debug.LogError("Loading not found");
        }
    }

    void Start() 
    {
        
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
        // currentChapter = startingChapter;
        // LoadNewChapter(currentChapter);
        StartCoroutine(StartLoading());
        if (tutorialFinished) {
            ChapterManager.Instance.InitGame();
        }
        OnReset?.Invoke();
        StartCoroutine(DoneLoading());
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

    public void LoadData(GameData data) {
        damageUpgrade = data.damageUpgrade;
        speedUpgrade = data.speedUpgrade;
        hpUpgrade = data.hpUpgrade;
        jumpUpgrade = data.jumpUpgrade;
        tutorialFinished = data.tutorialFinished;
        OnDataLoaded?.Invoke();
        if (tutorialFinished) {
            ChapterManager.Instance.InitGame();
        } else {
            ChapterManager.Instance.InitFirstTimePlay();
        }
        StartCoroutine(DoneLoading());
    }

    public void SaveData(GameData data) {
        data.damageUpgrade = damageUpgrade;
        data.speedUpgrade = speedUpgrade;
        data.hpUpgrade = hpUpgrade;
        data.jumpUpgrade = jumpUpgrade;
        data.tutorialFinished = tutorialFinished;
    }

    private void OnBuyDamage() {
        damageUpgrade += 1;
        OnDataLoaded?.Invoke();
    }

    private void OnBuySpeed() {
        speedUpgrade += 1;
        OnDataLoaded?.Invoke();
    }

    private void OnBuyHP() {
        hpUpgrade += 1;
        OnDataLoaded?.Invoke();
    }

    private void OnBuyJump() {
        jumpUpgrade += 1;
        OnDataLoaded?.Invoke();
    }

    private void OnTutorialFinished() {
        tutorialFinished = true;
    }
    IEnumerator StartLoading() {
        Loading.SetActive(true);
        yield return null;
    }

    IEnumerator DoneLoading() {
        yield return new WaitForSeconds(1.5f);
        Loading.SetActive(false);
    }

    private void OnEnable() {
        EndChapterScript.EndChapterZoneReached += OnEndChapterZoneReached;
        PlayerBehavior.OnPlayerDeath += Reset;
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
        UpgradeShopUI.OnBuyDamage += OnBuyDamage;
        UpgradeShopUI.OnBuySpeed += OnBuySpeed;
        UpgradeShopUI.OnBuyHP += OnBuyHP;
        UpgradeShopUI.OnBuyJump += OnBuyJump;
        EndzoneScript.OnTutorialFinished += OnTutorialFinished;
    }

    private void OnDisable() {
        EndChapterScript.EndChapterZoneReached -= OnEndChapterZoneReached;
        PlayerBehavior.OnPlayerDeath -= Reset;
        AugmentManager.OnAugmentPickup -= OnAugmentPickup;
        UpgradeShopUI.OnBuyDamage -= OnBuyDamage;
        UpgradeShopUI.OnBuySpeed -= OnBuySpeed;
        UpgradeShopUI.OnBuyHP -= OnBuyHP;
        UpgradeShopUI.OnBuyJump -= OnBuyJump;
        EndzoneScript.OnTutorialFinished -= OnTutorialFinished;
    }
}

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

    public Upgrades upgrades;

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
        _instance = this; ;
    }

    void Start() 
    {
        Reset();
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

    private int damageUpgrade;
    private int speedUpgrade;

    private int dashUpgrade;

    private int jumpUpgrade;

    public bool tutorialFinished;

    public void LoadData(GameData data) {
        damageUpgrade = data.upgrades.damageUpgrade;
        speedUpgrade = data.upgrades.speedUpgrade;
        dashUpgrade = data.upgrades.dashUpgrade;
        jumpUpgrade = data.upgrades.jumpUpgrade;
        tutorialFinished = data.tutorialdata.tutorialFinished;
    }

    public void SaveData(GameData data) {
        data.upgrades.damageUpgrade = damageUpgrade;
        data.upgrades.speedUpgrade = speedUpgrade;
        data.upgrades.dashUpgrade = dashUpgrade;
        data.upgrades.jumpUpgrade = jumpUpgrade;
        data.tutorialdata.tutorialFinished = tutorialFinished;
    }

    private void OnBuyDamage() {
        damageUpgrade += 1;
    }

    private void OnBuySpeed() {
        speedUpgrade += 1;
    }

    private void OnBuyDash() {
        dashUpgrade += 1;
    }

    private void OnBuyJump() {
        jumpUpgrade += 1;
    }

    private void OnTutorialFinished() {
        tutorialFinished = true;
    }

    private void OnEnable() {
        EndChapterScript.EndChapterZoneReached += OnEndChapterZoneReached;
        PlayerBehavior.OnPlayerDeath += Reset;
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
        UpgradeShopUI.OnBuyDamage += OnBuyDamage;
        UpgradeShopUI.OnBuySpeed += OnBuySpeed;
        UpgradeShopUI.OnBuyDash += OnBuyDash;
        UpgradeShopUI.OnBuyJump += OnBuyJump;
        EndzoneScript.OnTutorialFinished += OnTutorialFinished;
    }

    private void OnDisable() {
        EndChapterScript.EndChapterZoneReached -= OnEndChapterZoneReached;
        PlayerBehavior.OnPlayerDeath -= Reset;
        AugmentManager.OnAugmentPickup -= OnAugmentPickup;
        UpgradeShopUI.OnBuyDamage -= OnBuyDamage;
        UpgradeShopUI.OnBuySpeed -= OnBuySpeed;
        UpgradeShopUI.OnBuyDash -= OnBuyDash;
        UpgradeShopUI.OnBuyJump -= OnBuyJump;
        EndzoneScript.OnTutorialFinished -= OnTutorialFinished;
    }
}

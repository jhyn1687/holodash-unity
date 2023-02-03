using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static event Action OnReset;
    [SerializeField] private int startingChapter;
    [SerializeField] private TextMeshProUGUI AugmentPickupText;

    private int currentChapter;

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

    void Start() {
        currentChapter = startingChapter;
        ChapterManager.Instance.initChapter(currentChapter);
    }

    private void OnEndzoneReached() {
        currentChapter += 1;
        ChapterManager.Instance.initChapter(currentChapter);
    }

    private void Reset() {
        currentChapter = 0;
        ChapterManager.Instance.initChapter(currentChapter);
        OnReset?.Invoke();
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

    private void OnEnable() {
        EndzoneScript.EndzoneReached += OnEndzoneReached;
        PlayerBehavior.OnPlayerDeath += Reset;
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
    }
    private void OnDisable() {
        EndzoneScript.EndzoneReached -= OnEndzoneReached;
        PlayerBehavior.OnPlayerDeath += Reset;
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
    }
}

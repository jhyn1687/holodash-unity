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

    private ChapterManager chapterScript;

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
        chapterScript = GetComponent<ChapterManager>();
        chapterScript.initChapter(currentChapter);
    }

    private void OnEndzoneReached() {
        currentChapter += 1;
        chapterScript.initChapter(currentChapter);
    }

    private void Reset() {
        currentChapter = 0;
        chapterScript.initChapter(currentChapter);
        OnReset?.Invoke();
    }

    public void OnDeath() {
        currentChapter = 0;
        chapterScript.initChapter(currentChapter);
        OnReset?.Invoke();
    }
    public void OnAugmentPickup(Augment aug)
    {
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
    }
    private void OnDisable() {
        EndzoneScript.EndzoneReached -= OnEndzoneReached;
    }
}

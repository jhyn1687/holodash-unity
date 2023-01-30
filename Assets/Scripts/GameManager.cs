using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int CHAPTER;
    [SerializeField] private TextMeshProUGUI AugmentPickupText;

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
        chapterScript = GetComponent<ChapterManager>();
        InitGame();
    }

    void InitGame() {
        chapterScript.initChapter(CHAPTER);
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
}

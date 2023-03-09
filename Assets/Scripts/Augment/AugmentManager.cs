using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    public static event Action<int> OnAugmentPickup;
    public static event Action OnAugmentReset;
    private static AugmentManager _instance;

    public static List<Augment> allAugments;
    public static List<Augment> repeatableAugments;
    public static List<Augment> augmentPool;
    public static AugmentManager Instance {
        get {
            if (_instance == null) {
                Debug.Log("Augment Manager is null");
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        allAugments = new List<Augment>(Resources.LoadAll<Augment>(""));
        repeatableAugments = allAugments.FindAll(aug => aug.repeatable);
        augmentPool = allAugments.FindAll(aug => true);
        _instance = this;
    }
    public void AugmentPickup(Augment aug) {
        augmentPool.Remove(aug);
        if(augmentPool.Count == 0) {
            augmentPool = repeatableAugments.FindAll(aug => true);
        }
        OnAugmentPickup?.Invoke(aug.ID);
    }
    public Augment GetRandomAugment() {
        return augmentPool[UnityEngine.Random.Range(0, augmentPool.Count)];
    }
    public static string GetName(int code)
    {
        return allAugments.Find(aug => aug.ID == code).augmentName;
    }
    public static Augment GetAugment(int code) {
        return allAugments.Find(aug => aug.ID == code);
    }

    public void AugmentReset() {
        repeatableAugments = allAugments.FindAll(aug => aug.repeatable);
        augmentPool = allAugments.FindAll(aug => true);
        OnAugmentReset?.Invoke();
    }
}

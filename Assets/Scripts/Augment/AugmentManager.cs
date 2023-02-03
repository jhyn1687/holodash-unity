using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    public static event Action<int> OnAugmentPickup;
    private static AugmentManager _instance;

    public static List<Augment> allAugments;
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
        _instance = this;
    }
    public void AugmentPickup(Augment aug) {
        OnAugmentPickup?.Invoke(aug.ID);
    }

    public static string GetName(int code)
    {
        return allAugments.Find(aug => aug.ID == code).augmentName;
    }
    public static Augment GetAugment(int code) {
        return allAugments.Find(aug => aug.ID == code);
    }
}

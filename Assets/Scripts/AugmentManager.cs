using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentManager : MonoBehaviour
{
    private static AugmentManager _instance;

    public static List<Augment> allAugments;
    
    public List<Augment> augments;
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
    public static Augment GetID(int code)
    {
        return allAugments.Find(aug => aug.ID == code);
    }

    public void addAugment(Augment aug)
    {
        augments.Add(aug);
    }

    public void removeAugment(Augment aug)
    {
        augments.Remove(aug);
    }
    
    public bool hasAugment(Augment aug)
    {
        return augments.Contains(aug);
    }
}

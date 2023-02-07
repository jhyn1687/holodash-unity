using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] private Image abilityImage;
    
    // Start is called before the first frame update
    void Start()
    {
        abilityImage.fillAmount = 0;
    }

    public void AbilityUnready() {
        abilityImage.fillAmount = 1;
    }
    public void AbilityReady() {
        abilityImage.fillAmount = 0;
    }
    private void OnReset() {
        abilityImage.fillAmount = 0;
    }

    private void OnEnable() {
        GameManager.OnReset += OnReset;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnReset;
    }
}

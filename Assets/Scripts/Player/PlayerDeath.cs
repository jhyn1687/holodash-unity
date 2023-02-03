using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDeath : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    [SerializeField] private TextMeshProUGUI deathUI;
    // Start is called before the first frame update
    void OnDeath() {
        StartCoroutine(Die());
    }

    IEnumerator Die() {
        deathUI.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        deathUI.gameObject.SetActive(false);
        OnPlayerDeath?.Invoke();
    }
    
    private void OnReset() {
        StopAllCoroutines();
        deathUI.gameObject.SetActive(false);
    }

    private void OnEnable() {
        GameManager.OnReset += OnReset;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnReset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDeath : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI deathUI;
    // Start is called before the first frame update
    void OnDeath() {
        StartCoroutine(Die());
    }

    IEnumerator Die() {
        deathUI.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        deathUI.gameObject.SetActive(false);
        GameManager.Instance.OnDeath();
    }
    
    public void OnReset() {
        StopAllCoroutines();
        deathUI.gameObject.SetActive(false);
    }
}

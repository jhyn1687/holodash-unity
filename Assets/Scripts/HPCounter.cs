using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HPCounter : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textCoins;
    private HealthScript hs;

    void Start() {
        hs = GetComponent<HealthScript>();
    }

    // Start is called before the first frame update
    public void OnDamageTaken(float newHP) {
        textCoins.SetText("HP: " + newHP.ToString("F0"));
    }

    private void OnReset() {
        textCoins.SetText("HP: " + hs.GetMaxHP());
    }

    private void OnEnable() {
        GameManager.OnReset += OnReset;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnReset;
    }
}

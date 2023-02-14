using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBehavior : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image slider;
    [SerializeField] private Color low;
    [SerializeField] private Color high;
    [SerializeField] private Vector3 Offset;

    // Start is called before the first frame update
    public void setHealth(float health, float maxHealth) {
        slider.gameObject.SetActive(health < maxHealth);
        slider.fillAmount = health/ maxHealth;
        slider.color = Color.Lerp(low, high, health/ maxHealth);
    }

    private void Update() {
        if (objectToFollow != null) {
            rectTransform.position = objectToFollow.position + Offset;/*
            rectTransform.localScale = objectToFollow.localScale;*/
        }
    }
}

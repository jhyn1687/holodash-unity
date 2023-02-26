using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashIcon : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private Sprite offCD;
    [SerializeField] private Sprite onCD;

    public void Start() {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = offCD;
    }
    // Start is called before the first frame update
    public void OnDash()
    {
        if (sr != null) { 
            sr.sprite = onCD;
        }
    }
    public void OnDashRefilled() {
        if(sr != null)
        {
            sr.sprite = offCD;
        }
    }

    private void OnEnable() {
        GameManager.OnReset += OnDashRefilled;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnDashRefilled;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAnimation : MonoBehaviour
{
    private TrailRenderer tr;
    private ParticleSystem ps;
    private ParticleSystem.EmissionModule em;

    public void Start() {
        tr = GetComponent<TrailRenderer>();
        ps = GetComponentInChildren<ParticleSystem>();
        em = ps.emission;
    }
    // Start is called before the first frame update
    public void OnDash() {
        em.rateOverTime = 100;
        tr.emitting = true;
        
    }
    public void OnDashFinished() {
        tr.emitting = false;
        em.rateOverTime = 0;
    }

    private void OnEnable() {
        GameManager.OnReset += OnDashFinished;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnDashFinished;
    }
}

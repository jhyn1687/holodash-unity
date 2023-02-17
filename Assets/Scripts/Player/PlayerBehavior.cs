using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour , PlayerHealth {
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private TrailRenderer tr;
    private SpriteRenderer sr;
    private Animator ani;
    
    public static event Action OnPlayerDeath;
    
    [SerializeField] private float maxHP;
    [SerializeField] private float knockbackForce;
    [SerializeField] private TextMeshProUGUI deathUI;
    [SerializeField] private TextMeshProUGUI HPUI;
    [SerializeField] private HPBehavior HPBar;
    [SerializeField] private ParticleSystem dashEffect;

    public float Health { get; set; }
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        dead = false;
        Health = maxHP;
        HPUI.SetText("HP: " + maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) {
            return;
        }
        if (Health <= 0) {
            OnDeath();
        }
    }

    void FixedUpdate()
    {
        
    }

    private void OnEndZoneReached() {
        // reset player position
        this.transform.position = new Vector2(-14.5f, 5f);
    }

    private void OnReset() {
        // stop any dashes, and also reset everything that may have been changed in dash.
        StopAllCoroutines();
        ani.SetBool("Taking Damage", false);
        dead = false;
        Health = maxHP;
        HPBar.setHealth(Health, maxHP);
        HPUI.SetText(Health.ToString("F0") + " / " + maxHP.ToString("F0"));
    }

    void OnDeath() {
        dead = true;
        StartCoroutine(Die());
    }
    IEnumerator Die() {
        deathUI.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        deathUI.gameObject.SetActive(false);
        OnPlayerDeath?.Invoke();
    }
    // hackjob knockback
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.CompareTag("Enemy")) {
            Vector2 dir = (this.transform.position - collided.transform.position).normalized;
            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void Damage(float damage) {
        Health = Mathf.Max(Health - damage, 0);
        StartCoroutine(DamageAnimation());
        HPBar.setHealth(Health, maxHP);
        HPUI.SetText(Health.ToString("F0") + " / " + maxHP.ToString("F0"));
    }
    public void Heal(float healing) {
        
        Health = Mathf.Min(Health + healing, maxHP);
        HPBar.setHealth(Health, maxHP);
        HPUI.SetText(Health.ToString("F0") + " / " + maxHP.ToString("F0"));
    }
    public void DamageOverTime(float damage, float time) {
        StartCoroutine(DOT(damage, time));
    }
    IEnumerator DOT(float damage, float time) {
        float damageTaken = 0;
        while (damageTaken < damage) {
            Damage(damage / time);
            damageTaken += damage / time;
            yield return new WaitForSeconds(1f);
        }
    }
    IEnumerator DamageAnimation() {
        ani.SetBool("Taking Damage", true);
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("Taking Damage", false);
    }
    void OnAugmentPickup(int id) {
        switch (AugmentManager.GetName(id)) {
            case "Extra Health":
                maxHP += 50;
                Heal(50);
                break;
            default:
                break;
        }
    }
    
    private void OnEnable() {
        GameManager.OnReset += OnReset;
        EndChapterScript.EndChapterZoneReached += OnEndZoneReached;
        AugmentManager.OnAugmentPickup += OnAugmentPickup;
    }
    private void OnDisable() {
        GameManager.OnReset -= OnReset;
        EndChapterScript.EndChapterZoneReached -= OnEndZoneReached;
        AugmentManager.OnAugmentPickup -= OnAugmentPickup;
    }
}

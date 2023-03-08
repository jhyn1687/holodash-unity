using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Analytics;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour , PlayerHealth {
    private static PlayerBehavior _instance;
    public static PlayerBehavior Instance {
        get {
            if (_instance == null) {
                Debug.Log("Player Behavior is null");
            }
            return _instance;
        }
    }
    
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

    private float damageTaken = 1f;
    private float iframes = 0.5f;
    private float lastDamageTime;

    public float Health { get; set; }
    private bool dead;
    private Dictionary<string, object> deathData = new Dictionary<string, object>();

    void Awake() {
        _instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        tr = GetComponent<TrailRenderer>();
        sr = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        dead = false;
        lastDamageTime = 0;
        Health = maxHP;
        HPUI.SetText("HP: " + maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (dead || Time.timeScale == 0) {
            return;
        }
        lastDamageTime -= Time.deltaTime;
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
        maxHP = 50;
        Health = maxHP;
        damageTaken = 1f;
        HPBar.setHealth(Health, maxHP);
        HPUI.SetText(Health.ToString("F0") + " / " + maxHP.ToString("F0"));
    }

    void OnDeath() {
        dead = true;
        StartCoroutine(Die());
    }
    IEnumerator Die() {
        deathData["timeSinceStartup"] = Time.realtimeSinceStartup;
        AnalyticsService.Instance.CustomData("playerDeath", deathData);
        deathUI.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        deathUI.gameObject.SetActive(false);
        OnPlayerDeath?.Invoke();
    }
    // hackjob knockback
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.CompareTag("Enemy")) {
            Vector2 dir = this.transform.position - collided.transform.position;
            dir.Set(dir.x > 0 ? dir.x + 2 : dir.x - 2, dir.y);
            dir = dir.normalized;
            rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }
    }

    public void Damage(float damage) {
        if(lastDamageTime < 0) {
            damage = damage * damageTaken;
            Health = Mathf.Max(Health - damage, 0);
            StartCoroutine(DamageAnimation());
            HPBar.setHealth(Health, maxHP);
            HPUI.SetText(Health.ToString("F0") + " / " + maxHP.ToString("F0"));
            lastDamageTime = iframes;
        }
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
            case "Small Health Boost":
                maxHP += 10;
                Heal(15);
                break;
            case "Health Boost":
                maxHP += 25;
                Heal(30);
                break;
            case "Neuro Carapace":
                damageTaken *= 0.95f;
                Heal(10);
                break;
            case "Neuro Stim":
                Heal(50);
                break;
            case "Glass Cannon":
                damageTaken += .25f;
                break;
            case "Infused Bullets":
                maxHP -= 20;
                Heal(0);
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

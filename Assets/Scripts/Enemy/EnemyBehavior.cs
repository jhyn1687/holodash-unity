using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    public static event Action BossDied;

    public enum EnemyState { Idle, Patrol, Alert, Attack, Dead };
    public EnemyState currentState { get; set; }
    public Vector2 playerLastLocation { get; set; }

    private float damageAnimationTimer = 0f;
    private Animator ani;
    [SerializeField] private float damageToPlayer;
    [SerializeField] private GameObject coin;
    [SerializeField] private int maxHealth;
    [SerializeField] private EnemyHPBehavior HPBar;

    public float Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        playerLastLocation = Vector2.negativeInfinity;
        currentState = EnemyState.Idle;
        Health = maxHealth;
        damageAnimationTimer = 0f;
    }

    // Update is called once per frame
    void Update() 
    {
        if (Health <= 0) {
            OnDeath();
        }
        if (damageAnimationTimer < 0) {
            ani.SetBool("Taking Damage", false);
        } else {
            damageAnimationTimer -= Time.deltaTime;
        }
    }

    // when enemy dies, drop coin for player to pickup
    // later can incorporate different amounts of coins for
    // different enemies, etc.
    private void OnDeath() 
    {
        Instantiate(coin, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
        Debug.Log("enemy died: " + gameObject.name);
        
        // if this enemy is a boss
        if (String.Equals(gameObject.name, "Boss")) {
            BossDied?.Invoke();
            Debug.Log("Boss Died");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        GameObject collided = collision.gameObject;
        if (collided.TryGetComponent(out PlayerHealth hit)) 
        {
            hit.Damage(damageToPlayer);
        }
    }

    public void Damage(float damage) 
    {
        Health = Mathf.Max(Health - damage, 0);
        damageAnimationTimer = 0.2f;
        ani.SetBool("Taking Damage", true);
        HPBar.setHealth(Health, maxHealth);
    }

    public void DamageOverTime(float damage, float time, float timeMultiplier) 
    {
        StartCoroutine(DOT(damage, time * timeMultiplier, timeMultiplier));
    }

    IEnumerator DOT(float damage, float time, float timeMultiplier) 
    {
        float damageTaken = 0;
        while (damageTaken < damage) 
        {
            Damage(damage / time);
            damageTaken += damage / time;
            yield return new WaitForSeconds(1f * timeMultiplier);
        }
    }
}

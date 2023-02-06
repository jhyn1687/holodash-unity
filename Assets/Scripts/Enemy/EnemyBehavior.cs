using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    private Animator ani;
    public enum EnemyState { Idle, Patrol, Alert, Attack, Dead };
    public EnemyState currentState { get; set; }
    public Vector2 playerLastLocation { get; set; }
    [SerializeField] private float damageToPlayer;
    [SerializeField] private GameObject coin;
    [SerializeField] private int maxHealth;

    public float Health { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        playerLastLocation = Vector2.negativeInfinity;
        currentState = EnemyState.Idle;
        Health = maxHealth;
    }

    // Update is called once per frame
    void Update() {
        if (Health <= 0) {
            OnDeath();
        }
    }
    // when enemy dies, drop coin for player to pickup
    // later can incorporate different amounts of coins for
    // different enemies, etc.
    private void OnDeath() {
        Instantiate(coin, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.TryGetComponent(out PlayerHealth hit)) {
            hit.Damage(damageToPlayer);
        }
    }
    public void Damage(float damage) {
        Health -= damage;
        StartCoroutine(DamageAnimation());
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
}

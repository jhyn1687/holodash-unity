using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public BulletProperty bulletProps;
    private Rigidbody2D rb;
    private int ricochetCounter;
    private float lifetimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ricochetCounter = 0;
        lifetimeCounter = 0;

        rb.AddForce(this.transform.right * bulletProps.bulletSpeed);
    }

    private void Update()
    {
        lifetimeCounter += Time.deltaTime;
        if (ricochetCounter > bulletProps.ricochets)
        {
            Object.Destroy(this.gameObject);
        }
        if (lifetimeCounter > bulletProps.lifetime * bulletProps.LifetimeMultiplier) {
            Object.Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.TryGetComponent(out IDamageable hit)) {
            hit.Damage(bulletProps.damage * bulletProps.DamageMultiplier);
            hit.DamageOverTime(bulletProps.DOTDamage * bulletProps.DOTDamageMultiplier, bulletProps.DOTTime, bulletProps.DOTTimeMultiplier);
            if (Random.Range(0f, 1f) < bulletProps.lifestealChance) {
                PlayerBehavior.Instance.Heal(bulletProps.lifesteal);
            }
            Object.Destroy(this.gameObject);
        } 
        else if (collided.layer == LayerMask.NameToLayer("Ground"))
        {
            ricochetCounter++;
        } else if (collided.layer == LayerMask.NameToLayer("Enemy Projectile")) {
            Object.Destroy(collided.gameObject);
            Object.Destroy(this.gameObject);
        }
    }

    private void Reset() {
        // idk what to do here
    }

    void OnEnable() {
        GameManager.OnReset += Reset;
    }

    void OnDisable() {
        GameManager.OnReset -= Reset;
    }
}

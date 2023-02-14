using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public BulletProperty bulletProps;
    private Rigidbody2D rb;
    private int ricochetCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ricochetCounter = 0;

        rb.AddForce(this.transform.right * bulletProps.bulletSpeed);
    }

    private void Update()
    {
        if (ricochetCounter > bulletProps.ricochets)
        {
            Object.Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.TryGetComponent(out IDamageable hit)) {
            hit.Damage(bulletProps.damage);
            hit.DamageOverTime(bulletProps.DOTDamage * bulletProps.DOTDamageMultiplier, bulletProps.DOTTime * bulletProps.DOTTimeMultiplier);
            Object.Destroy(this.gameObject);
        } 
        else if (collided.layer == LayerMask.NameToLayer("Ground") && !string.Equals(collided.tag, "OneWayPlatform"))
        {
            ricochetCounter++;
        } else if (collided.layer == LayerMask.NameToLayer("Enemy Projectile")) {
            Object.Destroy(collided.gameObject);
            ricochetCounter++;
        }
    }

    private void Reset() {
        // idk what to do here
    }

    void OnEnable() {
        GameManager.OnReset += Reset;
    }

    void OnDisable() {
        GameManager.OnReset += Reset;
    }
}

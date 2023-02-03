using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public BulletProperty bulletProps;
    private Rigidbody2D rb;
    private int ricochetCounter;
    private float damage;
    private float DOTDamage;
    private float DOTTime;
    private float bulletSpeed;
    private int ricochets;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damage = bulletProps.damage;
        bulletSpeed = bulletProps.bulletSpeed;
        ricochets = bulletProps.ricochets;
        DOTDamage = bulletProps.DOTDamage;
        DOTTime = bulletProps.DOTTime;
        if (AugmentManager.Instance.hasAugment(AugmentManager.GetID(3)))
        {
            ricochets += 1;
        }

        rb.AddForce(this.transform.right * bulletSpeed);
    }

    private void Update()
    {
        if (ricochetCounter > ricochets)
        {
            Object.Destroy(this.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collided = collision.gameObject;
        if (collided.TryGetComponent(out IDamageable hit)) {
            hit.Damage(damage);
            if (AugmentManager.Instance.hasAugment(AugmentManager.GetID(1)))
            {
                hit.DamageOverTime(DOTDamage, DOTTime);
            }
            Object.Destroy(this.gameObject);
        } 
        else if (collided.layer == LayerMask.NameToLayer("Ground"))
        {
            ricochetCounter++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public BulletProperty bulletProps;
    private Rigidbody2D rb;
    private int ricochetCounter;
    private float damage;
    private float bulletSpeed;
    private int ricochets;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damage = bulletProps.damage;
        bulletSpeed = bulletProps.bulletSpeed;
        ricochets = bulletProps.ricochets;
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
        GameObject collider = collision.gameObject;
        if (collider.layer == LayerMask.NameToLayer(bulletProps.targetLayer)) {
            collider.GetComponent<HealthScript>().TakeDamage(damage);
            if (AugmentManager.Instance.hasAugment(AugmentManager.GetID(1)))
            {
                collider.GetComponent<HealthScript>().TakeDOT(5, 5);
            }
            Object.Destroy(this.gameObject);
        } 
        else if (collider.layer == LayerMask.NameToLayer("Ground"))
        {
            ricochetCounter++;
        }

    }
}

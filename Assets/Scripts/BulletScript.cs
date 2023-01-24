using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public BulletProperty bulletProps;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(this.transform.right * bulletProps.bulletSpeed);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.layer == LayerMask.NameToLayer(bulletProps.targetLayer)) {
            collider.GetComponent<HealthScript>().TakeDamage(bulletProps.damage);
            Object.Destroy(this.gameObject);
        } 
        else if (collider.layer == LayerMask.NameToLayer("Ground"))
        {
            Object.Destroy(this.gameObject);
        }
    }
}

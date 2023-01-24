using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Debug.Log(this.transform.rotation);
        rb.AddForce(this.transform.right * 600);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //rb.velocity = new Vector2(3, 0);
        //rb.AddForce(this.transform.right * 60);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.gameObject;
        if (collider.layer == LayerMask.NameToLayer("Ground"))
        {
            Object.Destroy(this.gameObject);
        }
    }
}

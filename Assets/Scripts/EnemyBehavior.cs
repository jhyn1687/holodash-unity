using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    [SerializeField] private float HP;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(float damage) {
        HP -= damage;
    }
}

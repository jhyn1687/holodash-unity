using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator ani;
    [SerializeField] private GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    // when enemy dies, drop coin for player to pickup
    // later can incorporate different amounts of coins for
    // different enemies, etc.
    public void OnDeath() {
        Instantiate(coin, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }
    
    public void OnDamageTaken() {
        StartCoroutine(DamageAnimation());
    }

    IEnumerator DamageAnimation() {
        ani.SetBool("Taking Damage", true);
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("Taking Damage", false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.CompareTag("Player")) {
            collided.GetComponent<HealthScript>().TakeDamage(10);
        }
    }
}

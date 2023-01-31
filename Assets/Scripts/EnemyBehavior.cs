using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private bool isQuitting = false;
    [SerializeField] private GameObject coin;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void OnApplicationQuit() {
        isQuitting = true;
    }

    // when enemy dies, drop coin for player to pickup
    // later can incorporate different amounts of coins for
    // different enemies, etc.
    void OnDestroy() {
        if (!isQuitting) {
            Instantiate(coin, this.transform.position, this.transform.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour {
    protected static string[] layerNames = new string[] { "Ground", "Player" };
    protected Transform player;
    [SerializeField] protected GameObject enemy;
    protected SpriteRenderer sr;
    protected EnemyBehavior eb;
    protected Rigidbody2D rb;
    [Header("Patrol")]
    [SerializeField] protected float speed;
    [SerializeField] protected float distance;
    [SerializeField] protected Transform _frontCheckPoint;
    [SerializeField] protected Transform _backCheckPoint;
    [SerializeField] protected Transform _wallCheckPoint;
    [SerializeField] protected Vector2 _checkSize;
    [SerializeField] protected EnemyBehavior.EnemyState defaultState = EnemyBehavior.EnemyState.Patrol;


    protected bool movingLeft;
    protected bool canPatrol;
    protected RaycastHit2D frontGroundInfo;
    protected RaycastHit2D backGroundInfo;
    protected RaycastHit2D wallInfo;
    // Start is called before the first frame update
    void Start() {
        player = null;
        sr = enemy.GetComponent<SpriteRenderer>();
        eb = enemy.GetComponent<EnemyBehavior>();
        rb = enemy.GetComponent<Rigidbody2D>();
        movingLeft = true; 
        canPatrol = true;
    }
    protected void Run() {
        enemy.transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject collided = collision.gameObject;
        if (collided.CompareTag("Player")) {
            player = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        GameObject collided = collision.gameObject;
        if (collision.CompareTag("Player")) {
            player = null;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontCheckPoint.position, _checkSize);
        Gizmos.DrawWireCube(_backCheckPoint.position, _checkSize);
        Gizmos.DrawWireCube(_wallCheckPoint.position, _checkSize);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : EnemyAI {
    // Start is called before the first frame update
    void Start() {
        player = null;
        sr = enemy.GetComponent<SpriteRenderer>();
        eb = enemy.GetComponent<EnemyBehavior>();
        rb = enemy.GetComponent<Rigidbody2D>();
        movingLeft = true; 
        canPatrol = true;
    }

    void Update() {
        // sometimes they freak out if they're stuck in a 2x2 L shaped block??? idk how to fix
        frontGroundInfo = Physics2D.BoxCast(_frontCheckPoint.position, _checkSize, 0f, Vector2.down, 0f, LayerMask.GetMask("Ground"));
        backGroundInfo = Physics2D.BoxCast(_backCheckPoint.position, _checkSize, 0f, Vector2.down, 0f, LayerMask.GetMask("Ground"));
        string[] layers = new string[] { "Ground", "Enemy" };
        wallInfo = Physics2D.BoxCast(_wallCheckPoint.position, _checkSize, 0f, Vector2.down, 0f, LayerMask.GetMask(layers));
    }
    void FixedUpdate()
    {
        if (eb.currentState == EnemyBehavior.EnemyState.Patrol || eb.currentState == EnemyBehavior.EnemyState.Idle) {
            if (frontGroundInfo.collider == null && backGroundInfo.collider == null) {
                // can't patrol
                canPatrol = false;
            } else if (frontGroundInfo.collider != null && wallInfo.collider != null && backGroundInfo.collider == null) {
                canPatrol = false;
            } else {
                canPatrol = true;
            }
            if (canPatrol && eb.currentState == EnemyBehavior.EnemyState.Patrol)
            {
                Run();
            }
            if (canPatrol && (frontGroundInfo.collider == null || (wallInfo.collider != null))) {
                if (movingLeft == true) {
                    enemy.transform.eulerAngles = new Vector3(0, 0, 0);
                    movingLeft = false;
                } else {
                    enemy.transform.eulerAngles = new Vector3(0, -180, 0);
                    movingLeft = true;
                }
            }
        }
        if (player != null) {
            LayerMask layerMask = LayerMask.GetMask(layerNames);
            Vector2 dir = player.position - transform.position;
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Vector2.Distance(transform.position, player.position), layerMask);
            if (hit.collider != null) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                    Debug.DrawRay(transform.position, dir.normalized * hit.distance, Color.yellow);
                    eb.currentState = EnemyBehavior.EnemyState.Alert;
                    if (!inProx) {
                        player = null;
                    }
                } else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")) {
                    Debug.DrawRay(transform.position, dir.normalized * hit.distance, Color.red);
                    if (dir.x < 0f) {
                        enemy.transform.eulerAngles = new Vector3(0, 0, 0);
                    } else {
                        enemy.transform.eulerAngles = new Vector3(0, -180, 0);
                    }
                    eb.currentState = EnemyBehavior.EnemyState.Attack;
                    eb.playerLastLocation = player.position;
                }
            }
        } else {
            eb.currentState = canPatrol ? defaultState : EnemyBehavior.EnemyState.Idle;
            eb.playerLastLocation = Vector2.negativeInfinity;
        }
    }
}

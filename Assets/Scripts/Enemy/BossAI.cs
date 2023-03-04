using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : EnemyAI {
    // Start is called before the first frame update
    void Start() {
        
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
        
    }
}

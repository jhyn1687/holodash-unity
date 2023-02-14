using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    private void OnBossDied()
    {
        GameObject.Destroy(gameObject);
    }

    private void OnEnable()
    {
        EnemyBehavior.BossDied += OnBossDied;
    }

    private void OnDisable()
    {
        EnemyBehavior.BossDied -= OnBossDied;
    }
}

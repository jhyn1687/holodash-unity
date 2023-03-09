using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{

    // true = gate appears when something happens
    // false = gate destroys when something happens
    [SerializeField] bool reverse;

    void Start() {
        if (reverse)
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void OnPlayerDeath()
    {
        Debug.Log("gate destroyed");
        GameObject.Destroy(gameObject);

    }

    private void OnStartRun()
    {
        Debug.Log("gate at " + gameObject.transform.position + "visible");
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnBossDied()
    {
        GameObject.Destroy(gameObject);
    }

    private void OnEnable()
    {
        EnemyBehavior.BossDied += OnBossDied;
        PlayerBehavior.OnPlayerDeath += OnPlayerDeath;
        ChapterManager.StartRun += OnStartRun;

    }

    private void OnDisable()
    {
        EnemyBehavior.BossDied -= OnBossDied;
        PlayerBehavior.OnPlayerDeath -= OnPlayerDeath;
        ChapterManager.StartRun -= OnStartRun;

    }
}

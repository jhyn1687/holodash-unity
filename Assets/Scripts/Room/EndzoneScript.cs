using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndzoneScript : MonoBehaviour
{
    public static event Action EndzoneReached;
    public static event Action OnTutorialFinished;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player") {
            Debug.Log("Exit reached");
            EndzoneReached?.Invoke();
            OnTutorialFinished?.Invoke();
            GameObject.Destroy(gameObject);
        }
    }
}

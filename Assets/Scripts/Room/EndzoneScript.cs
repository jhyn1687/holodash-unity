using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndzoneScript : MonoBehaviour
{
    public static event Action EndzoneReached;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player") {
            EndzoneReached?.Invoke();
        }
    }
}

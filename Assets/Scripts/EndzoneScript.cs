using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndzoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player") {
            GameManager.Instance.OnEndZoneReached();
            GameObject player = GameObject.FindWithTag("Player");
            player.BroadcastMessage("OnEndZoneReached");
        }
    }
}

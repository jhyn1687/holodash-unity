using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndzoneScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player") {
            Debug.Log("trigger entered by player");
            GameManager.Instance.OnEndZoneReached();
        }
    }
}

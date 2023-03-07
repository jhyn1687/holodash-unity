using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndzoneScript : MonoBehaviour
{
    public static event Action EndzoneReached;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Player") {

            // check if in room
            GameObject gm = GameObject.Find("GameManager");
            ChapterManager cm = gm.GetComponent<ChapterManager>();
            if (cm.doorLevel > 0)
            {
                cm.DestroyDoorRoom();
            } else {
                Debug.Log("Exit reached");
                EndzoneReached?.Invoke();
                GameObject.Destroy(gameObject);
            }
        }
    }
}

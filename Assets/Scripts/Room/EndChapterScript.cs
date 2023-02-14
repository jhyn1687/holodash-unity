using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndChapterScript : MonoBehaviour
{
    public static event Action EndChapterZoneReached;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            EndChapterZoneReached?.Invoke();
            GameObject.Destroy(gameObject);
        }
    }

}

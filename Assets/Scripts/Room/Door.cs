using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private GameObject room;

    private GameObject curr;

    private void OnTriggerStay2D(Collider2D collision)
    {
        curr = collision.gameObject;
        // Debug.Log("Enter collision: " + collision.gameObject.tag + " " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player") && Input.GetButtonDown("Crouch"))
        {
            init();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Debug.Log("Exit collision: " + collision.gameObject.tag + " " + collision.gameObject.name);
        curr = null;
    }

    private void init()
    {
        Debug.Log("Door entered");
        // DoorEntered?.Invoke();

        GameObject gm = GameObject.Find("GameManager");
        gm.GetComponent<ChapterManager>().InitDoorRoom(room, this.transform.position);
    }

}

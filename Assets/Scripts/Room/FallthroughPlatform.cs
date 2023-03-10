using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// from this dude: https://www.youtube.com/watch?v=7rCUt6mqqE8

public class FallthroughPlatform : MonoBehaviour
{
    private GameObject currentOneWayPlatform;

    [SerializeField] private BoxCollider2D playerCollider;

    private void Update()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (currentOneWayPlatform != null)
            {
                Debug.Log("Fallthrough");
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("Enter collision: " + collision.gameObject.tag + " " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Debug.Log("Exit collision: " + collision.gameObject.tag + " " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}

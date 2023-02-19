using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Indicator : MonoBehaviour
{
    public GameObject indicatorPanel;

    public bool playerIsClose;

    // Update is called once per frame
    void Update()
    {
        if (playerIsClose)
        {
            indicatorPanel.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.E) && indicatorPanel.activeInHierarchy)
        {
            indicatorPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }
}


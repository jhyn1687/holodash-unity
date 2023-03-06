using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public UpgradeShopUI shop;

    public float wordSpeed;
    private bool playerIsClose;
    private bool shopIsOpen;

    void Start()
    {
        GameObject HUD = GameObject.Find("HUD");
        if (HUD == null) {
            Debug.LogError("HUD not found");
        }
        dialoguePanel = HUD.transform.Find("DialoguePanel").gameObject;
        if (dialoguePanel == null) {
            Debug.LogError("DialoguePanel not found");
        }
        dialoguePanel.SetActive(false);
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        if (dialogueText == null) {
            Debug.LogError("DialogueText not found");
        }
        shop = HUD.transform.Find("UpgradeShopUI").GetComponent<UpgradeShopUI>();
        if (shop == null) {
            Debug.LogError("UpgradeShopUI not found");
        }
        shop.gameObject.SetActive(false);
        dialogueText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interact")) {
            if (shopIsOpen) {
                closeShop();
            } else if (dialoguePanel.activeInHierarchy && playerIsClose) {
                openShop();
                clearText();
            } else if (playerIsClose) {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            clearText();
        }
    }

    public void clearText()
    {
        dialogueText.text = "";
        StopAllCoroutines();
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach(char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    private void openShop() 
    {
        shopIsOpen = true;
        shop.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void closeShop() {
        shop.gameObject.SetActive(false);
        shopIsOpen = false;
        Time.timeScale = 1;
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
            clearText();
            shop.gameObject.SetActive(false);
        }
    }
}

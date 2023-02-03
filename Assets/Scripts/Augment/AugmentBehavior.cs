using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentBehavior : MonoBehaviour
{

    private BoxCollider2D coll;

    [SerializeField] private Augment aug;
    [SerializeField] private GameObject iconObject;
    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        SpriteRenderer sr = iconObject.GetComponent<SpriteRenderer>();
        sr.sprite = aug.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        // make it bob up and down? maybe?
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            AugmentManager.Instance.AugmentPickup(this.aug);
            Object.Destroy(this.gameObject);
        }
    }
}

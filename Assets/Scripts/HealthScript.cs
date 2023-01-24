using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    [SerializeField] private float HP;
    // Start is called before the first frame update
    void Update() {
        if (HP <= 0) {
            Destroy(this.gameObject);
        }
    }
    // Update is called once per frame
    public void TakeDamage(float damage) {
        HP -= damage;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    [SerializeField] private float HP;
    private 
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

    public void TakeDOT(float damage, int time)
    {
        StartCoroutine(DamageOverTime(damage, time));
    }
    IEnumerator DamageOverTime(float damage, int time)
    {
        float damageTaken = 0;
        while (damageTaken < damage)
        {
            TakeDamage(damage / time);
            damageTaken += damage / time;
            yield return new WaitForSeconds(1f);
        }
    }
}

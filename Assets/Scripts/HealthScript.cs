using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

    [SerializeField] private float maxHP;
    private float HP;
    private bool hasBroadcasted;
    private Animator ani;

    void Start() {
        hasBroadcasted = false;
        HP = maxHP;
        ani = GetComponent<Animator>();
    }
    void Update() {
        if (HP <= 0 && !hasBroadcasted) {
            gameObject.BroadcastMessage("OnDeath");
            hasBroadcasted = true;
        }
    }
    IEnumerator DamageAnimation() {
        ani.SetBool("damage taken", true);
        yield return new WaitForSeconds(0.5f);
        ani.SetBool("damage taken", false);
    }

    // Update is called once per frame
    public void TakeDamage(float damage) {
        HP -= damage;
        StartCoroutine(DamageAnimation());
        gameObject.BroadcastMessage("OnDamageTaken", HP);
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

    public void OnReset() {
        hasBroadcasted = false;
        HP = maxHP;
    }

    public float GetCurrentHP() {
        return HP;
    }
}

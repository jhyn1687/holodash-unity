using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerHealth
{
    float Health { get; set; }
    void Damage(float damage);
    void Heal(float healing);
    void DamageOverTime(float damage, float time);
}

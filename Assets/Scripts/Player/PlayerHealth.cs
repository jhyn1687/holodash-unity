using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerHealth
{
    float Health { get; set; }
    void Damage(float damage);
    void DamageOverTime(float damage, float time);
}

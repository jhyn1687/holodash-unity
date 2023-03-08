using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    float Health { get; set; }
    void Damage(float damage);
    void DamageOverTime(float damage, float time, float timeMultiplier);
}

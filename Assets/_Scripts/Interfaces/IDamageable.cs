using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamageable
{
    bool Damage(float amount);

    void Die();
    bool IsDamageable { get; set; }

    float MaxHealth { get; set; }
    float CurrentHealth { get; set; }
}

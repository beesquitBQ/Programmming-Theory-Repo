using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Sword : Weapon
{
    // POLYMORPHISM
    protected override void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        if (enemy is Slime)
        {
            damage *= damageMultiplier;
        }
        enemy.TakeDamage(damage, this);
    }
}

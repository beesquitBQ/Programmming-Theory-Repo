using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// INHERITANCE
public class Hammer : Weapon
{
    // POLYMORPHISM
    protected override void Start()
    {
        base.Start();
        attackDuration = 1.5f;

    }

    // POLYMORPHISM
    protected override void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        if (enemy is Skeleton)
        {
            damage *= damageMultiplier;
        }
        enemy.TakeDamage(damage, this);
    }
}

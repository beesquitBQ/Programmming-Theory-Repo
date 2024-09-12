using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    protected override void Start()
    {
        base.Start();
        attackDuration = 0.7f;

    }

    protected override void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        if (enemy is Skeleton)
        {
            damage *= vulnerabilityDamageMultiplier;
        }
        enemy.TakeDamage(damage);
    }
}

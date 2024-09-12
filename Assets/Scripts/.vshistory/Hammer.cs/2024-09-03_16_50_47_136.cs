using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    protected override void Attack()
    {
        Debug.Log("Hammer Attack");
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

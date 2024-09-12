using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        if (enemy is Slime)
        {
            damage *= damageMultiplier;
            enemy.TakeDamage(damage);
        }
        else
        {
            enemy.TakeDamage(damage);
        }
    }
}

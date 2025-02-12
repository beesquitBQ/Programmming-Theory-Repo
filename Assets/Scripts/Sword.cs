using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private AudioClip slimeHitSound; // Звук удара по слайму
    [SerializeField] private AudioClip skeletonHitSound; // Звук удара по скелету
    [SerializeField] private AudioClip regularHitSound; // Обычный звук удара

    protected override void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        if (enemy is Slime)
        {
            damage *= damageMultiplier;
        }
        enemy.TakeDamage(damage, this);
    }

    // Переопределяем методы для получения звуков
    protected override AudioClip GetSpecialHitSound(Enemy enemy)
    {
        if (enemy is Slime)
        {
            return slimeHitSound; // Звук разрубания слайма
        }
        else if (enemy is Skeleton)
        {
            return skeletonHitSound; // Глухой звук удара по скелету
        }
        return null;
    }

    protected override AudioClip GetRegularHitSound()
    {
        return regularHitSound; // Обычный звук удара
    }
}
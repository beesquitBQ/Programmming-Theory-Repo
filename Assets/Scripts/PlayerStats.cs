using UnityEngine;
using System;
using System.Collections;

// INHERITANCE
public class PlayerStats : Creature
{
    // POLYMORPHISM
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    // POLYMORPHISM
    public override void Die()
    {
        base.Die();
        if (gameController != null)
        {
            gameController.GameOver();
        }
    }

    // POLYMORPHISM
    protected override void OnDeathComplete()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.Die();
        }
        this.enabled = false;
    }
}
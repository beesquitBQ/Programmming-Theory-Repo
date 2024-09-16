using UnityEngine;
using System;
using System.Collections;

public class PlayerStats : Creature
{
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    public override void Die()
    {
        base.Die();
        if (gameController != null)
        {
            gameController.GameOver();
        }
    }

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
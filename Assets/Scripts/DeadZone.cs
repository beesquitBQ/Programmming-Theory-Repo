using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private const float PLAYER_DAMAGE = 9999f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(PLAYER_DAMAGE);
            }
        }
        else
        {
            Creature creature = collision.gameObject.GetComponent<Creature>();

            if (creature != null)
            {
                creature.Die();
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
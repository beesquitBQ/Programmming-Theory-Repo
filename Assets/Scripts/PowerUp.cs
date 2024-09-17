using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    // ENCAPSULATION
    [SerializeField] private float healAmount = 20f;

    // ABSTRACTION
    private void OnTriggerEnter(Collider other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.Heal(healAmount);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private float maxDamage = 8.0f;
    private float minDamage = 4.0f;
    private float vulnerabilityDamageMultiplier = 1.5f;
    private float reduceDamageMultipliar = 0.5f;
    private PlayerStats playerStats;
    protected bool isBlocking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isBlocking = Input.GetMouseButton(1);
    }

    void Attack()
    {

    }

    void ApplyDamage(float damageAmount)
    {
        if (isBlocking)
        {
            damageAmount *= reduceDamageMultipliar;
        }
        playerStats.PlayerTakeDamage(damageAmount);
    }
}

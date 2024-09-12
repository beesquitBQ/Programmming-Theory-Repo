using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float maxDamage = 8.0f;
    [SerializeField] protected float minDamage = 4.0f;
    [SerializeField] protected float vulnerabilityDamageMultiplier = 1.5f;
    [SerializeField] protected float reduceDamageMultiplier = 0.5f;
    
    protected PlayerStats playerStats;
    protected bool isBlocking = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        isBlocking = Input.GetMouseButton(1);
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }

    protected abstract void Attack();
    
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = Random.Range(minDamage, maxDamage);
                ApplyDamageToEnemy(enemy, damage);
            }
        }
    }

    protected abstract void ApplyDamageToEnemy(Enemy enemy, float damage);

    protected virtual void ApplyDamage(float damageAmount)
    {
        if (isBlocking)
        {
            damageAmount *= reduceDamageMultiplier;
        }
        playerStats.PlayerTakeDamage(damageAmount);
    }
}

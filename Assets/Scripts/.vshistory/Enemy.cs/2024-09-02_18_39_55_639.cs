using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private float turningSpeed = 30.0f;
    [SerializeField] protected float maxHealth = 20f;
    protected float currentHealth;
    private float dealDamage;
    protected GameObject player;
    protected Rigidbody enemyRb;
    protected PlayerStats playerStats;
    protected Weapon weapon;

    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        LookDirection();
    }

    protected void LookDirection()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
        }
        
    }

    protected virtual void TakeDamage()
    {
        // логика получения врагом урона
    }

    protected virtual void DealDamage()
    {
        // логика нанесения врагом урона
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // обработка столкновений врага
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        // обработка завершения столкновений врага
    }
}
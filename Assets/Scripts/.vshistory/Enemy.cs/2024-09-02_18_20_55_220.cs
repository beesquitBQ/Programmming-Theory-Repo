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
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.transform.Rotate(lookDirection * turningSpeed * Time.deltaTime);
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
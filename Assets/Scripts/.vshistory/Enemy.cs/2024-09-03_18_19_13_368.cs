using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float turningSpeed = 30.0f;
    [SerializeField] protected float maxHealth = 20f;
    protected float currentHealth;
    private float dealDamage = 5f;
    protected GameObject player;
    protected Rigidbody enemyRb;
    protected PlayerStats playerStats;
    protected Weapon weapon;

    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        playerStats = player.GetComponent<PlayerStats>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        LookDirection();
        //Move();
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

    protected virtual void Move()
    {
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        enemyRb.MovePosition(transform.position + directionToPlayer * movementSpeed * Time.deltaTime);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= dealDamage;
        if (currentHealth<=0)
        {
            Die();
        }
    }

    protected virtual void DealDamage()
    {
        if (playerStats != null)
        {
            playerStats.PlayerTakeDamage(dealDamage);
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            DealDamage();
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {

    }
}
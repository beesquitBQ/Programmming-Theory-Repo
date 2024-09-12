using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float turningSpeed = 30.0f;
    [SerializeField] protected float maxHealth = 50f;
    protected float currentHealth;
    private float dealDamage = 5f;
    protected GameObject player;
    protected Rigidbody enemyRb;
    protected PlayerStats playerStats;
    protected Weapon weapon;

    [SerializeField] private float invulnerabilityDuration = 0.3f;
    private bool isInvulnerable = false;

    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (!isInvulnerable)
        {
            currentHealth -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvulnerabilityCoroutine());
            }
        }
        
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    public virtual void DealDamageToPlayer(PlayerStats playerStats)
    {
        if (playerStats != null && IsAttacking())
        {
            playerStats.PlayerTakeDamage(dealDamage);
        }
    }

    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} has been defeated!");
        Destroy(gameObject);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public abstract bool IsAttacking();

    protected virtual void OnCollisionEnter(Collision collision)
    {
        
    }

    protected virtual void OnCollisionExit(Collision collision)
    {

    }
}
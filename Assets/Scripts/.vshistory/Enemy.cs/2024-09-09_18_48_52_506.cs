using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Creature
{
    [SerializeField] protected float movementSpeed = 3f;
    [SerializeField] protected float turningSpeed = 30.0f;
    [SerializeField] protected int scoreValue = 100;
    protected float scoreMultiplier = 1.25f;

    private float dealDamage = 5f;
    protected GameObject player;
    protected Rigidbody enemyRb;
    protected PlayerStats playerStats;
    protected Weapon weapon;
    protected bool isKilled = false;

    protected override void Start()
    {
        base.Start();
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();
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

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage, Weapon attackingWeapon)
    {
        float damageToApply = damage;
        if (attackingWeapon != null)
        {
            damageToApply *= attackingWeapon.damageMultiplier;
        }
        base.TakeDamage(damageToApply);
        if (currentHealth <= 0 && !isKilled)
        {
            isKilled = true;

            if (attackingWeapon != null)
            {
                playerStats.IncreaseScore(Mathf.RoundToInt(scoreValue * attackingWeapon.scoreMultiplier));
            }
            else
            {
                playerStats.IncreaseScore(scoreValue);
            }
        }
    }

    protected override void Die()
    {
        OnDeath?.Invoke(0f);
        Debug.Log($"{gameObject.name} has been defeated!");
        isKilled = false;
        Destroy(gameObject);
    }

    public virtual void DealDamageToPlayer(PlayerStats playerStats)
    {
        if (playerStats != null && IsAttacking())
        {
            playerStats.TakeDamage(dealDamage);
        }
    }

    public abstract bool IsAttacking();

    protected virtual void OnCollisionEnter(Collision collision) { }
    protected virtual void OnCollisionExit(Collision collision) { }

}
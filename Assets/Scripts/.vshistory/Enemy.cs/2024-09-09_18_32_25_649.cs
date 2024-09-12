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

    public virtual void TakeDamage(float damage, Weapon weapon)
    {
        base.TakeDamage(damage);
        if (currentHealth <= 0)
        {
            if (weapon != null)
            {
                playerStats.IncreaseScore(Mathf.RoundToInt(scoreValue * scoreMultiplier));
            }
            else
            {
                playerStats.IncreaseScore(scoreValue);
            }
        }
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
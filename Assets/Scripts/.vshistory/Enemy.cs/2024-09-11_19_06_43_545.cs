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
            int scoreToAdd = attackingWeapon != null
                ? Mathf.RoundToInt(scoreValue * attackingWeapon.scoreMultiplier)
                : scoreValue;
            GameManager.Instance.IncreaseScore(scoreToAdd);
        }
    }

    public override void Die()
    {
        base.Die();
    }

    protected override IEnumerator DeathCoroutine()
    {
        yield return base.DeathCoroutine();
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
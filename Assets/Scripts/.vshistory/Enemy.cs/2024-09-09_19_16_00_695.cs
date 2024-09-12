using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Creature
{
    [SerializeField] protected float movementSpeed = 3f;
    [SerializeField] protected float turningSpeed = 30.0f;
    private float dealDamage = 5f;
    protected GameObject player;
    protected Rigidbody enemyRb;
    protected PlayerStats playerStats;
    protected bool isKilled = false;
    protected int scoreValue = 10; // Базовое количество очков за убийство противника

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

    //protected virtual void Move()
    //{
    //    Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
    //    enemyRb.MovePosition(transform.position + directionToPlayer * movementSpeed * Time.deltaTime);
    //}

    public virtual void DealDamageToPlayer(PlayerStats playerStats)
    {
        if (playerStats != null && IsAttacking())
        {
            playerStats.TakeDamage(dealDamage);
        }
    }

    public virtual void TakeDamage(float damage, Weapon attackingWeapon)
    {
        float damageToApply = damage;
        if (attackingWeapon != null)
        {
            // Применяем дополнительный урон, если есть оружие
            damageToApply *= attackingWeapon.damageMultiplier;
        }
        base.TakeDamage(damageToApply);
        if (currentHealth <= 0 && !isKilled)
        {
            isKilled = true; // Отмечаем, что враг был убит
            // Увеличиваем очки игрока в зависимости от оружия
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

    public abstract bool IsAttacking();

    protected virtual void OnCollisionEnter(Collision collision) { }

    protected virtual void OnCollisionExit(Collision collision) { }

    protected override void Die()
    {
        OnDeath?.Invoke(0f);
        Debug.Log($"{gameObject.name} has been defeated!");
        isKilled = false; // Сбрасываем флаг, чтобы можно было начислять очки при следующем убийстве
        Destroy(gameObject);
    }
}
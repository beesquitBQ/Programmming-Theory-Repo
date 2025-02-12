using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Creature
{
    // ENCAPSULATION
    [SerializeField] protected float turningSpeed = 120.0f;
    [SerializeField] protected int scoreValue = 100;
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected float detectionRadius = 10f;
    [SerializeField] protected float wanderRadius = 10f;
    [SerializeField] protected float wanderTimer = 2f;

    protected float dealDamage = 5f;
    protected GameObject player;
    protected Rigidbody enemyRb;
    protected PlayerStats playerStats;
    protected Weapon weapon;
    protected NavMeshAgent agent;

    protected bool isAttacking = false;
    protected float lastAttackTime = 0f;
    protected float wanderTime = 0f;

    // POLYMORPHISM
    protected override void Start()
    {
        base.Start();
        enemyRb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player?.GetComponent<PlayerStats>();

        if (animator == null)
        {
            Debug.LogError("Animator component not found on Enemy!");
        }
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent not found on Enemy!");
        }
        else
        {
            agent.updateRotation = false;
        }
        if (player == null)
        {
            Debug.LogError("Player not found!");
        }

        if (enemyRb != null)
        {
            enemyRb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    // ABSTRACTION
    protected virtual void Update()
    {
        if (!isDead)
        {
            HandleMovement();
            UpdateAnimation();
        }
    }

    // ABSTRACTION
    protected virtual void HandleMovement()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer <= detectionRadius)
            {
                if (agent.enabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(player.transform.position);
                }
                LookAtPlayer();

                if (CanAttack())
                {
                    Attack();
                }
            }
            else
            {
                Wander();
            }
        }
    }

    // ABSTRACTION
    protected virtual void LookAtPlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            directionToPlayer.y = 0;
            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turningSpeed * Time.deltaTime);
            }
        }
    }

    // ABSTRACTION
    protected virtual void Wander()
    {
        wanderTime -= Time.deltaTime;

        if (wanderTime <= 0f)
        {
            Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            Vector3 finalPosition = hit.position;

            if (agent.enabled && agent.isOnNavMesh)
            {
                agent.SetDestination(finalPosition);
            }

            wanderTime = wanderTimer;
        }
    }

    // ENCAPSULATION
    protected virtual bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown;
    }

    // ABSTRACTION
    protected virtual void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
    }

    // POLYMORPHISM
    public virtual void TakeDamage(float damage, Weapon attackingWeapon)
    {
        float damageToApply = damage;
        if (attackingWeapon != null)
        {
            damageToApply *= attackingWeapon.damageMultiplier;
        }
        base.TakeDamage(damageToApply);

        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    // POLYMORPHISM
    public override void Die()
    {
        if (!isDead)
        {
            isDead = true;
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
            if (agent != null)
            {
                agent.isStopped = true;
            }
            if (gameController != null)
            {
                int scoreToAdd = weapon != null
                    ? Mathf.RoundToInt(scoreValue * weapon.scoreMultiplier)
                    : scoreValue;
                gameController.IncreaseScore(scoreToAdd);
            }
            StartCoroutine(DeathCoroutine());
        }
    }

    // ABSTRACTION
    protected virtual void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool("IsMoving", agent.velocity.magnitude > 0.1f);
            animator.SetBool("IsAttacking", isAttacking);
        }
    }

    // ABSTRACTION
    public virtual void DealDamageToPlayer(PlayerStats playerStats)
    {
        if (playerStats != null && isAttacking)
        {
            playerStats.TakeDamage(dealDamage);
        }
    }

    // ENCAPSULATION
    public virtual bool IsAttacking()
    {
        return isAttacking;
    }

    // ABSTRACTION
    protected virtual IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

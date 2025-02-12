using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

// INHERITANCE
public class Skeleton : Enemy
{
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float fieldOfView = 85f;
    [SerializeField] private float eyeHeight;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 20f;
    [SerializeField] private Transform shootPoint;

    private static readonly int IsMovingParam = Animator.StringToHash("IsMoving");
    private static readonly int AttackTrigger = Animator.StringToHash("Attack");
    private static readonly int DieTrigger = Animator.StringToHash("Die");

    private bool isPerformingAttack = false;

    protected override void Start()
    {
        base.Start();
        attackCooldown = 2f;
        wanderTime = wanderTimer;
    }

    protected override void Update()
    {
        if (!isDead)
        {
            HandleMovement();
            UpdateAnimation();
        }
    }
    private bool CanSeePlayer()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position - (Vector3.up * eyeHeight);
            float distanceToPlayer = directionToPlayer.magnitude;
            if (distanceToPlayer < sightDistance)
            {
                float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
                if (angleToPlayer < fieldOfView * 0.5f)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), directionToPlayer);
                    if (Physics.Raycast(ray, out RaycastHit hit, sightDistance))
                    {
                        if (hit.transform.gameObject == player)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
    protected override void HandleMovement()
    {
        if (player != null && !isDead && !isPerformingAttack)
        {
            if (CanSeePlayer())
            {
                LookAtPlayer();
                float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
                if (CanAttack() && distanceToPlayer <= detectionRadius)
                {
                    StartCoroutine(PerformAttack());
                }
                else if (distanceToPlayer > attackCooldown)
                {
                    if (agent.enabled && agent.isOnNavMesh)
                    {
                        agent.SetDestination(player.transform.position);
                    }
                }
                else
                {
                    agent.ResetPath();
                }
            }
            else
            {
                Wander();
            }
        }
    }

    private IEnumerator PerformAttack()
    {
        isPerformingAttack = true;
        agent.ResetPath();

        // Запускаем анимацию атаки
        if (animator != null)
        {
            animator.SetTrigger(AttackTrigger);
        }

        // Ждем завершения атаки (время анимации)
        yield return new WaitForSeconds(attackCooldown);

        // Завершаем атаку
        isPerformingAttack = false;
        lastAttackTime = Time.time;

        // Если игрок все еще в зоне поражения, повторяем атаку
        if (CanSeePlayer() && CanAttack())
        {
            StartCoroutine(PerformAttack());
        }
    }

    public void OnArrowLaunchEvent()
    {
        ShootArrow();
    }

    private void ShootArrow()
    {
        if (arrowPrefab != null && shootPoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
            if (arrowRb != null)
            {
                arrowRb.velocity = shootPoint.forward * arrowSpeed;
            }
        }
    }

    protected override void UpdateAnimation()
    {
        if (animator != null)
        {
            animator.SetBool(IsMovingParam, agent.velocity.magnitude > 0.1f);
        }
    }

    public override void Die()
    {
        if (!isDead)
        {
            base.Die();
            if (agent != null)
            {
                agent.isStopped = true;
                agent.enabled = false;
            }
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
            }
            if (animator != null)
            {
                animator.SetTrigger(DieTrigger);
            }
            StartCoroutine(DeathCoroutine());
        }
    }

    protected override IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Enemy
{
    [SerializeField] private float sightDistance = 20f;
    [SerializeField] private float fieldOfView = 85f;
    [SerializeField] private float eyeHeight;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private float arrowSpeed = 20f;
    [SerializeField] private Transform shootPoint;

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsAttackingHash = Animator.StringToHash("IsAttacking");
    private static readonly int DieHash = Animator.StringToHash("Die");
    private static readonly int IdleHash = Animator.StringToHash("Idle");

    protected override void Start()
    {
        base.Start();
        attackCooldown = 1f;
    }

    protected override void HandleMovement()
    {
        if (player != null && !isDead)
        {
            if (CanSeePlayer())
            {
                LookAtPlayer();

                if (CanAttack())
                {
                    Attack();
                }

                if (agent.enabled && agent.isOnNavMesh && Vector3.Distance(transform.position, player.transform.position) > attackCooldown)
                {
                    agent.SetDestination(player.transform.position);
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

    protected override void Attack()
    {
        base.Attack();
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
            animator.SetBool(IsMovingHash, agent.velocity.magnitude > 0.1f);
            animator.SetBool(IsAttackingHash, isAttacking);

            if (agent.velocity.magnitude <= 0.1f && !isAttacking)
            {
                animator.SetTrigger(IdleHash);
            }
        }
    }

    protected override IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

// INHERITANCE
public class Slime : Enemy
{
    // ENCAPSULATION
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float forwardJumpMultiplier = 1.5f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float jumpDistance = 4f;
    [SerializeField] private float deathDelay = 1.5f;
    private float lastJumpTime = 0f;

    // POLYMORPHISM
    protected override void HandleMovement()
    {
        if (isDead) return;
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer <= detectionRadius)
            {
                LookAtPlayer();
                if (distanceToPlayer <= jumpDistance && CanJump())
                {
                    JumpToPlayer();
                }
                else if (agent != null && agent.enabled && agent.isOnNavMesh)
                {
                    agent.SetDestination(player.transform.position);
                }
            }
            else
            {
                Wander();
            }
        }
    }

    // ENCAPSULATION
    private bool CanJump()
    {
        return Time.time - lastJumpTime >= jumpCooldown;
    }

    // ABSTRACTION
    private void JumpToPlayer()
    {
        if (agent == null || player == null || enemyRb == null) return;
        isAttacking = true;
        lastJumpTime = Time.time;
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        directionToPlayer.y = 0;
        StartCoroutine(JumpCoroutine(directionToPlayer));
    }

    // ABSTRACTION
    private System.Collections.IEnumerator JumpCoroutine(Vector3 direction)
    {
        if (agent != null) agent.enabled = false;
        Vector3 jumpVector = Vector3.up * jumpForce + direction * (jumpForce * forwardJumpMultiplier);
        enemyRb.AddForce(jumpVector, ForceMode.Impulse);
        if (animator != null) animator.SetTrigger("Jump");
        yield return new WaitForSeconds(0.5f);
        if (agent != null && !isDead)
        {
            agent.enabled = true;
            agent.Warp(transform.position);
        }
        isAttacking = false;
        if (animator != null) animator.SetTrigger("Land");
    }

    // POLYMORPHISM
    protected override void UpdateAnimation()
    {
        base.UpdateAnimation();
        if (animator != null)
        {
            animator.SetBool("IsJumping", isAttacking);
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
                agent.enabled = false;
            }
            if (enemyRb != null)
            {
                enemyRb.isKinematic = true;
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

    // POLYMORPHISM
    protected override IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    // POLYMORPHISM
    protected override void LookAtPlayer()
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

    // POLYMORPHISM
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DealDamageToPlayer(playerStats);
        }
    }

    // POLYMORPHISM
    protected override void Wander()
    {
        base.Wander();
    }
}
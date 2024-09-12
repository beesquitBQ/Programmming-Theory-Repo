using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.5f;
    [SerializeField] private float upwardJumpFactor = 0.5f;
    private float lastJumpTime = 0f;
    private bool isGrounded = false;

    protected override void Start()
    {
        maxHealth = 30f;
        base.Start();
        
    }

    protected override void Update()
    {
        base.LookDirection();
        CheckGrounded();

        if (isGrounded && Time.time - lastJumpTime >= jumpCooldown)
        {
            if (CanJumpToPlayer())
            {
                JumpToPlayer();
                lastJumpTime = Time.time;
            }
        }
    }

    private bool CanJumpToPlayer()
    {
        return true;
    }

    private void JumpToPlayer()
    {
        Vector3 direectionToPlayer = (player.transform.position - transform.position).normalized;
        Vector3 jumpDirection = new Vector3(direectionToPlayer.x, upwardJumpFactor, direectionToPlayer.z);
        jumpDirection = jumpDirection.normalized;

        enemyRb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
    }

    private void CheckGrounded()
    {
        isGrounded = true;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject == player)
        {
            playerStats.PlayerTakeDamage(Random.Range(4, 7));
        }
        else if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
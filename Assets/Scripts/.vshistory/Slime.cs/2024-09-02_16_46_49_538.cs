using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.5f;
    private float lastJumpTime = 0f;
    private bool isGrounded = true;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        LookDirection();
        if (Time.time - lastJumpTime >= jumpCooldown && CanJumpToPlayer)
        {
            JumpToPlayer();
            lastJumpTime = Time.time;
        }
    }
    private bool CanJumpToPlayer()
    {
        if (isGrounded)
        {
            return true;
        }
    }
    private void JumpToPlayer()
    {
        enemyRb.AddForce((player.transform.position - transform.position).normalized * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            PlayerStats.TakeDamage(Random.Range(4, 7));
        }
    }
}

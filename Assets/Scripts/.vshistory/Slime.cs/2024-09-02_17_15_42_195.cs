using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private float maxHealth = 30f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float jumpCooldown = 0.5f;
    private float lastJumpTime = 0f;
    private bool isGrounded = false;

    protected override void Start()
    {
        base.Start();
        currentHealth = maxHealth;
    }

    protected override void Update()
    {
        base.LookDirection();
        CheckGrounded();

        if (isGrounded && Time.time - lastJumpTime >= jumpCooldown) // ���� ����� �� ����� � ������ ���������� ������� � ���������� ������
        {
            if (CanJumpToPlayer()) // ���� ���� � ������ ��������
            {
                JumpToPlayer(); // ����� ������� � ������
                lastJumpTime = Time.time; // ���������� ����� ���������� ������
            }
        }
    }

    private bool CanJumpToPlayer()
    {
        // ������ ��������, ����� �� ��������� ������ � ������
        return true;
    }

    private void JumpToPlayer()
    {
        // ������ ������ � ������
        enemyRb.AddForce((player.transform.position - transform.position).normalized * jumpForce, ForceMode.Impulse);
    }

    private void CheckGrounded()
    {
        // ������ ��������, ����� �� ����� �� �����
        // ��������, ��������� raycast ��� collider
        isGrounded = true;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject == player) // ���� ����� ���������� � �������
        {
            // ��������� ����� ������
            playerStats.TakeDamage(Random.Range(4, 7)); // ������� ��������� ���� �� 4 �� 6
        }
        else if (collision.gameObject.tag == "Ground") // ���� ����� ����������� �� �����
        {
            isGrounded = true; // ��������, ��� ����� �� �����
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") // ���� ����� ��������� �� �����
        {
            isGrounded = false; // ��������, ��� ����� ������ �� �� �����
        }
    }
}
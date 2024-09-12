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

        if (isGrounded && Time.time - lastJumpTime >= jumpCooldown) // если Слайм на земле и прошло достаточно времени с последнего прыжка
        {
            if (CanJumpToPlayer()) // если путь к игроку свободен
            {
                JumpToPlayer(); // Слайм прыгает к игроку
                lastJumpTime = Time.time; // запоминаем время последнего прыжка
            }
        }
    }

    private bool CanJumpToPlayer()
    {
        // логика проверки, можно ли совершить прыжок к игроку
        return true;
    }

    private void JumpToPlayer()
    {
        // логика прыжка к игроку
        enemyRb.AddForce((player.transform.position - transform.position).normalized * jumpForce, ForceMode.Impulse);
    }

    private void CheckGrounded()
    {
        // логика проверки, стоит ли Слайм на земле
        // например, используя raycast или collider
        isGrounded = true;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject == player) // если Слайм столкнулся с игроком
        {
            // нанесение урона игроку
            playerStats.TakeDamage(Random.Range(4, 7)); // наносим случайный урон от 4 до 6
        }
        else if (collision.gameObject.tag == "Ground") // если Слайм приземлился на землю
        {
            isGrounded = true; // отмечаем, что Слайм на земле
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground") // если Слайм оторвался от земли
        {
            isGrounded = false; // отмечаем, что Слайм больше не на земле
        }
    }
}
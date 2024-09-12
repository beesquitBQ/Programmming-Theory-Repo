using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : Weapon
{
    [SerializeField] private Animator hammerAnimator;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private float attackDuration = 0.5f;

    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
        if (hammerAnimator == null)
        {
            hammerAnimator = GetComponent<Animator>();
        }
        if (swordCollider == null)
        {
            swordCollider = GetComponent<Collider>();
        }
        swordCollider.enabled = false;
        swordCollider.isTrigger = true;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            Attack();
        }

    }

    protected override void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        hammerAnimator.SetTrigger("Attack");
        swordCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        swordCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Sword collided with: {other.gameObject.name}");
        if (other.CompareTag("Enemy") && isAttacking)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = Random.Range(minDamage, maxDamage);
                ApplyDamageToEnemy(enemy, damage);
            }
            else
            {
                Debug.LogWarning("Collided with Enemy tagged object, but no Enemy component found!");
            }
        }
    }

    protected override void ApplyDamageToEnemy(Enemy enemy, float damage)
    {
        if (enemy is Slime)
        {
            damage *= vulnerabilityDamageMultiplier;
        }
        enemy.TakeDamage(damage);
    }
}

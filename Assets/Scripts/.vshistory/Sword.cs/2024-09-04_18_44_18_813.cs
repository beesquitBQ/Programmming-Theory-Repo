using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private float attackDuration = 0.5f;
    [SerializeField] Transform swordPosition;

    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
        if (swordAnimator == null)
        {
            swordAnimator = GetComponent<Animator>();
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
        if (Input.GetMouseButtonDown(0) && !isAttacking )
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
        swordAnimator.SetTrigger("Attack");
        swordCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        swordCollider.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && isAttacking)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = Random.Range(minDamage, maxDamage);
                ApplyDamageToEnemy(enemy, damage);
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

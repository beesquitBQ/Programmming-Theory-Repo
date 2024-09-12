using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    [SerializeField] private Animator swordAnimator;
    [SerializeField] private Collider swordCollider;
    [SerializeField] private float attackDuration = 0.5f;

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
        Debug.Log("Sword Attack");
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        swordAnimator.Play("SwordAnimation");
        swordCollider.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        swordCollider.enabled = false;
        isAttacking = false;
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

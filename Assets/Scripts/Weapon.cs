using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float maxDamage = 8.0f;
    [SerializeField] protected float minDamage = 4.0f;
    [SerializeField] protected float reduceDamageMultiplier = 0.5f;
    [SerializeField] protected float attackDuration = 0.5f;
    public float damageMultiplier = 1.5f;
    public float scoreMultiplier = 1.25f;
    protected PlayerStats playerStats;
    protected bool isBlocking = false;
    protected bool isAttacking = false;
    protected Animator weaponAnimator;
    protected Collider weaponCollider;

    protected virtual void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        weaponAnimator = GetComponent<Animator>();
        weaponCollider = GetComponent<Collider>();
        InitializeCollider();
    }

    protected virtual void Update()
    {
        isBlocking = Input.GetMouseButton(1);
        if (Input.GetMouseButton(0) && !isAttacking)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if (!isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    protected virtual IEnumerator PerformAttack()
    {
        isAttacking = true;
        weaponAnimator.SetTrigger("Attack");
        weaponCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        weaponCollider.enabled = false;
        isAttacking = false;
    }

    public bool IsAttacking() => isAttacking;

    private void OnDisable()
    {
        isAttacking = false;
        if (weaponAnimator != null)
        {
            weaponAnimator.ResetTrigger("Attack");
            weaponAnimator.Play("Idle");
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
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

    protected abstract void ApplyDamageToEnemy(Enemy enemy, float damage);

    protected virtual void ApplyDamage(float damageAmount)
    {
        if (isBlocking)
        {
            damageAmount *= reduceDamageMultiplier;
        }
        playerStats.TakeDamage(damageAmount);
    }

    protected virtual void InitializeCollider()
    {
        if (weaponCollider != null)
        {
            weaponCollider.enabled = false;
            weaponCollider.isTrigger = true;
        }
    }
}
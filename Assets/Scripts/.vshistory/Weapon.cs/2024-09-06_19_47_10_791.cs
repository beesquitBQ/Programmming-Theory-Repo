using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected float maxDamage = 8.0f;
    [SerializeField] protected float minDamage = 4.0f;
    [SerializeField] protected float vulnerabilityDamageMultiplier = 1.5f;
    [SerializeField] protected float reduceDamageMultiplier = 0.5f;
    [SerializeField] protected float attackDuration = 0.5f;
    
    protected PlayerStats playerStats;
    protected bool isBlocking = false;
    protected bool isAttacking = false;
    protected Animator weaponAnimator;
    protected Collider weaponCollider;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        weaponAnimator = GetComponent<Animator>();
        weaponCollider = GetComponent<Collider>();
        InitializeCollider();
    }

    // Update is called once per frame
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
        if(weaponCollider != null)
        {
            weaponCollider.enabled = false;
            weaponCollider.isTrigger = true;
        }
    }

    //public virtual void ResetWeaponState()
    //{
    //    isAttacking = false;
    //    if (weaponAnimator != null)
    //    {
    //        weaponAnimator.Rebind();
    //        weaponAnimator.Update(0f);
    //    }
    //    if (weaponCollider != null)
    //    {
    //        weaponCollider.enabled = false;
    //    }
    //}
}

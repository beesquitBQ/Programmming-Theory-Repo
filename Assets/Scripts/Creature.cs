using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// INHERITANCE
public abstract class Creature : HealthSystem
{
    protected Animator animator;
    protected GameController gameController;

    // POLYMORPHISM
    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }

    // POLYMORPHISM
    public override void Die()
    {
        if (!isDead)
        {
            base.Die();
            if (animator != null)
            {
                animator.SetTrigger("Die");
            }
            StartCoroutine(DeathCoroutine());
        }
    }

    // ABSTRACTION
    protected virtual IEnumerator DeathCoroutine()
    {
        if (animator != null)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            yield return new WaitForSeconds(1f);
        }
        OnDeathComplete();
    }

    // ABSTRACTION
    protected virtual void OnDeathComplete()
    {
        Destroy(gameObject);
    }
}
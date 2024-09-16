using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Creature : HealthSystem
{
    protected Animator animator;
    protected GameController gameController;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        gameController = FindObjectOfType<GameController>();
    }

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

    protected virtual void OnDeathComplete()
    {
        Destroy(gameObject);
    }
}
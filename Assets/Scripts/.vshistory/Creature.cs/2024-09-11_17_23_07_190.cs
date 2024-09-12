using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    protected float currentHealth;

    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnDeath;

    [SerializeField] private float invulnerabilityDuration = 0.3f;
    private bool isInvulnerable = false;
    [SerializeField] protected Animator deathAnimator;
    protected bool isDead;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(float amount)
    {
        if (!isInvulnerable)
        {
            currentHealth -= amount;
            OnHealthChanged?.Invoke(currentHealth / maxHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvulnerabilityCoroutine());
            }
            Debug.Log($"{gameObject.name} took {amount} damage. Current health {currentHealth}");
        }
    }

    protected IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    public virtual void Die()
    {
        if (!isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
            if (deathAnimator != null)
            {
                deathAnimator.SetTrigger("Die");
            }
            Debug.Log($"{gameObject.name} has been defeated!");
            StartCoroutine(DeathCoroutine());
        }
    }

    protected virtual IEnumerator DeathCoroutine()
    {
        if (deathAnimator != null)   
        {
            yield return new WaitForSeconds(deathAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(gameObject);
    }

    
    public float GetHealthPercentage()
    {
        return currentHealth/maxHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
    // ENCAPSULATION
    [SerializeField] protected float maxHealth = 100;
    protected float currentHealth;
    public UnityEvent<float> OnHealthChanged;
    public UnityEvent OnDeath;
    [SerializeField] protected float invulnerabilityDuration = 0.3f;
    protected bool isInvulnerable = false;
    protected bool isDead;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // POLYMORPHISM
    public virtual void TakeDamage(float amount)
    {
        if (!isInvulnerable && !isDead)
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

    // ABSTRACTION
    protected virtual IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    // POLYMORPHISM
    public virtual void Die()
    {
        if (!isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
            Debug.Log($"{gameObject.name} has been defeated!");
        }
    }

    //// ENCAPSULATION
    //public float GetHealthPercentage() => currentHealth / maxHealth;

    //// POLYMORPHISM
    //public virtual void ResetHealth()
    //{
    //    currentHealth = maxHealth;
    //    isDead = false;
    //    OnHealthChanged?.Invoke(currentHealth / maxHealth);
    //}

    // POLYMORPHISM
    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth / maxHealth);
        Debug.Log($"{gameObject.name} healed for {amount}. Current health {currentHealth}");
    }
}

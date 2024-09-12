using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Creature : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100;
    protected float currentHealth;

    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnDeath;

    [SerializeField] private float invulnerabilityDuration = 0.3f;
    private bool isInvulnerable = false;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
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

    protected virtual void Die()
    {
        OnDeath?.Invoke(0f);
        Debug.Log($"{gameObject.name} has been defeated!");
        Destroy(gameObject);
    }
    
    public float GetHealthPercentage()
    {
        return currentHealth/maxHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] protected float playerMaxHealth = 100;
    private float playerCurrentHealth;

    public UnityEvent<float> OnHealthChanged;
    public UnityEvent<float> OnDeath;

    [SerializeField] private float invulnerabilityDuration = 0.3f;
    private bool isInvulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
    }

    public void PlayerTakeDamage(float amount)
    {
        if (!isInvulnerable)
        {
            playerCurrentHealth -= amount;
            OnHealthChanged?.Invoke(playerCurrentHealth / playerMaxHealth);
            if (playerCurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(InvulnerabilityCoroutine());
            }
            Debug.Log($"Player took damage. Current health: {playerCurrentHealth}");
        }
    }

    private IEnumerator InvulnerabilityCoroutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    private void Die()
    {
        OnDeath?.Invoke(0f);
        Destroy(gameObject);
    }

    public float GetHealthPersentage()
    {
        return playerCurrentHealth/playerMaxHealth;
    }

}

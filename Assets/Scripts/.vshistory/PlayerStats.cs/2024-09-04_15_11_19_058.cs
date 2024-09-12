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

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
    }

    public void PlayerTakeDamage(float amount)
    {
        playerCurrentHealth -= amount;
        OnHealthChanged?.Invoke(playerCurrentHealth/playerMaxHealth);    

        if(playerCurrentHealth <= 0)
        {
            Die();
        }
        Debug.Log(playerCurrentHealth);
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

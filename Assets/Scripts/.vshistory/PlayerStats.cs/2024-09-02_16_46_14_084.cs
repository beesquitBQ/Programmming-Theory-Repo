using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    protected float playerMaxHealth = 100;
    protected    float playerCurrentHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
    }

    public void TakeDamage()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

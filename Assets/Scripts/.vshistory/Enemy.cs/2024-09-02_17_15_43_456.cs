using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]protected float movementSpeed;
    protected float turningSpeed = 30.0f;
    [SerializeField]protected float maxHealth = 20f;
    protected float currentHealth;
    private float dealDamade;

    protected GameObject player;
    protected Rigidbody enemyRb;

    protected PlayerStats playerStats;
    protected Weapon weapon;

    // Start is called before the first frame update
    protected void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    protected void LookDirection()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.transform.Rotate(lookDirection * turningSpeed * Time.deltaTime);
    }

    protected void TakeDamage()
    {

    }

    protected void DealDamage()
    {

    }

}

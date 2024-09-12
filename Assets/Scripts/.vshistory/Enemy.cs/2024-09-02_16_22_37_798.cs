using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private float movementSpeed;
    private float turningSpeed = 30.0f;
    [SerializeField]private float maxHealth = 20f;
    private float currentHealth;
    private float dealDamade;

    private GameObject player;
    private Rigidbody enemyRb;

    private PlayerStats playerStats;
    private Weapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void LookDirection()
    {
        Vector3 lookDirection = (player.transform.position - transform.position).normalized;
        enemyRb.transform.Rotate(lookDirection * turningSpeed * Time.deltaTime);
    }

    private void TakeDamage()
    {

    }

    private void DealDamage()
    {

    }

}

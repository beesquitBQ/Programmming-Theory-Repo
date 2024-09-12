using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]private float movementSpeed;
    private float turningSpeed = 30;
    [SerializeField]private float maxHealth;
    private float currentHealth;
    private float dealDamade;
    private GameObject player;
    private Rigidbody enemyRb;

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
}

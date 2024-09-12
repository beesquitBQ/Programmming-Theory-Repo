using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkeletonProjectile : MonoBehaviour
{
    public float damage = 10f;
    public float pushForce = 5f;
    public float lifeTime = 3f;
    public float destroyDelay = 0.1f;

    private bool hasHit = false;

    private void Start()
    {
        StartCoroutine(DestroyAfterLifetime());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats playerStats = collision.gameObject.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                PushPlayer(collision.gameObject);
            }
            StartCoroutine(DestroyProjectile());
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(DestroyProjectile());
        }
    }

    private void PushPlayer(GameObject player)
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            Vector3 pushDirection = (player.transform.position - transform.position).normalized;
            playerRb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }

    private IEnumerator DestroyProjectile()
    {
        hasHit = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifeTime);
        if (!hasHit)
        {
            Destroy(gameObject);
        }
    }
}
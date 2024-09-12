using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton : Enemy
{
    private NavMeshAgent agent;
    private Vector3 lastKnownPos;
    private Animator skeletonAttack;

    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }
    public Vector3 LastKnownPos { get => lastKnownPos; set => lastKnownPos = value; }
    private Vector3 wanderTarget;
    private float wanderRadius = 10f;
    private float minWanderTime = 3f;
    private float maxWanderTime = 8f;
    private float nextWanderTime;
    private bool isShooting = false;

    [Header("Sight values")]
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;

    [Header("Weapon values")]
    public Transform gunBarrel;
    [Range(0.1f, 10f)]
    public float fireRate;
    private float nextFireTime;
    public GameObject arrowPrefab;
    public float arrowSpeed = 20f;

    protected override void Start()
    {
        maxHealth = 20f;
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        SetNewWanderTarget();
        skeletonAttack = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.LookDirection();
        if (CanSeePlayer())
        {
            agent.SetDestination(transform.position);
            ShootAtPlayer();
        }
        else
        {
            Wander();
        }
    }

    private void Wander()
    {
        if (Time.time >= nextWanderTime)
        {
            SetNewWanderTarget();
        }

        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(wanderTarget);
        }
    }

    private void SetNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        wanderTarget = hit.position;

        nextWanderTime = Time.time + Random.Range(minWanderTime, maxWanderTime);
    }


    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            return true;
                        }
                    }

                    Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red, 0.1f);
                }
            }
        }
        return false;
    }

    private void MoveRandomly()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 2f;
        randomDirection.y = 0;
        agent.SetDestination(transform.position + randomDirection);
    }

    private void ShootAtPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            isShooting = true;
            skeletonAttack.SetTrigger("Attacking");
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
            StartCoroutine(ResetShootingState());
        }
    }

    private void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, gunBarrel.position, Quaternion.identity);
        Vector3 shootDirection = gunBarrel.forward;

        Rigidbody arrowRb = arrow.GetComponent<Rigidbody>();
        arrowRb.AddForce(shootDirection * arrowSpeed, ForceMode.Impulse);
    }

    private IEnumerator ResetShootingState()
    {
        yield return new WaitForSeconds(0.1f);
        isShooting = false;
        skeletonAttack.ResetTrigger("Attacking");
    }

    public override bool IsAttacking()
    {
        return isShooting;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

    }
}

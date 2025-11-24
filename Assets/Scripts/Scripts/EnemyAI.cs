using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System;
using URandom = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    private PlayerHealth playerHealth;

    [Header("Layers")]
    [SerializeField] private LayerMask terrainLayer;
    [SerializeField] private LayerMask playerLayerMask;

    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius = 10f;
    private Vector3 currentPatrolPoint;
    private bool hasPatrolPoint;

    [Header("Combat Settings")]
    [SerializeField] private float attackCooldown = 1f;
    private bool isOnAttackCooldown;
    [SerializeField] private float patrolSpeed = 2.5f;
    [SerializeField] private float chaseSpeed = 5.0f;
    [SerializeField] private int damageAmount = 10;

    [Header("Detection Ranges")]
    [SerializeField] private float visionRange = 20f;
    [SerializeField] private float engagementRange = 10f;

    private bool isPlayerVisible;
    private bool isPlayerInRange;


    private void Awake()
    {
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.Find("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
        if (navAgent == null)
        {
            navAgent = GetComponent<NavMeshAgent>();
        }
        if (playerTransform) playerHealth = playerTransform.GetComponent<PlayerHealth>();
    }
    private void Update()
    {
        DetectPlayer();
        UpdateBehaviorState();

    }

    private void DetectPlayer()
    {
        isPlayerVisible = Physics.CheckSphere(transform.position, visionRange, playerLayerMask);
        isPlayerInRange = Physics.CheckSphere(transform.position, engagementRange, playerLayerMask);
    }




    private IEnumerator AttackCooldownRoutine()
    {
        isOnAttackCooldown = true;
        yield return new WaitForSeconds(attackCooldown);
        isOnAttackCooldown = false;
    }


    private void DamagePlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.DamagePlayer(damageAmount);

        }
        else if (playerTransform != null)
        {
            playerTransform.SendMessage("DamagePlayer", damageAmount, SendMessageOptions.DontRequireReceiver);
            Debug.Log("Player taken damage (fallback)");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isOnAttackCooldown && other.CompareTag("Player"))
        {
            DamagePlayer();
            StartCoroutine(AttackCooldownRoutine());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!isOnAttackCooldown && collision.collider.CompareTag("Player"))
        {
            DamagePlayer();
            StartCoroutine(AttackCooldownRoutine());
        }
    }
    private void PerformChase()
    {
        navAgent.speed = chaseSpeed;
        if (playerTransform != null)
        {
            navAgent.SetDestination(playerTransform.position);
        }
    }

    private void PerformPatrol()
    {
        navAgent.speed = patrolSpeed;
        if (!hasPatrolPoint)
            FindPatrolPoint();


        if (hasPatrolPoint)
            navAgent.SetDestination(currentPatrolPoint);


        if (Vector3.Distance(transform.position, currentPatrolPoint) < 1f)
            hasPatrolPoint = false;

    }

    private void FindPatrolPoint()
    {
        float randomX = URandom.Range(-patrolRadius, patrolRadius);
        float randomZ = URandom.Range(-patrolRadius, patrolRadius);


        Vector3 potentialPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);


        if (Physics.Raycast(potentialPoint, -transform.up, 2f, terrainLayer))
        {
            currentPatrolPoint = potentialPoint;
            hasPatrolPoint = true;
        }

    }

    private void UpdateBehaviorState()
    {
        if (!isPlayerVisible && !isPlayerInRange)
        {
            PerformPatrol();
        }
        else if (isPlayerVisible && !isPlayerInRange)
        {
            PerformChase();
        }
        else if (isPlayerVisible && isPlayerInRange)
        {
            PerformChase();

        }
    }
}

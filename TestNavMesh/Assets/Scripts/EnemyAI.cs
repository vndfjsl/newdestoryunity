using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    NavMeshAgent agent;
    Transform playerTransform;

    public LayerMask whatIsGround, whatIsPlayer;
    public Vector3 walkPoint;
    bool bWalkPointSet;
    public float walkPointRange;

    public float timeBetFire;
    bool bAlreadyFire;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool bPlayerInSightRange, bPlayerInAttackRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.Find("Player").transform;
    }

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
    }

    // Update is called once per frame
    void Update()
    {
        bPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        bPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!bPlayerInSightRange && !bPlayerInAttackRange)
        {
            Patrolling();
        }
        else if (bPlayerInSightRange && !bPlayerInAttackRange)
        {
            ChasePlayer();
        }
        else if (bPlayerInSightRange && bPlayerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patrolling()
    {
        if (!bWalkPointSet)
        {
            SearchWalkPoint();
        }
        else
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distToWalkPoint = transform.position - walkPoint;
        if(distToWalkPoint.sqrMagnitude <= 1f)
        {
            bWalkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 pos = transform.position;
        walkPoint = new Vector3(pos.x + randomX, pos.y, pos.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            bWalkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(playerTransform.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(playerTransform);
        if(!bAlreadyFire)
        {
            GameObject clone = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rigid = clone.GetComponent<Rigidbody>();
            rigid.AddForce(transform.forward * 32f, ForceMode.Impulse); ;
            rigid.AddForce(transform.up * 4f, ForceMode.Impulse);

            bAlreadyFire = true;
            Invoke("ResetAttack", timeBetFire);
        }
    }

    private void ResetAttack()
    {
        bAlreadyFire = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}

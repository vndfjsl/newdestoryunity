using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField]
    private float remainDistanceMin = 1f; // 목적지까지의 거리가 1 미만이면

    private int destPoint = 0;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.autoBraking = false; // 종점가까이가면느려지는거
            GotoNextPoint();
        }
    }


    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= remainDistanceMin)
        {
            // false면 준비된거. 길계산되면
            GotoNextPoint();
        }
    }
    private void GotoNextPoint()
    {
        if(patrolPoints.Length == 0)
        {
            Debug.LogError("1개필요");
            enabled = false;
            return;
        }

        agent.destination = patrolPoints[destPoint].position;

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField]
    private float remainDistanceMin = 1f; // ������������ �Ÿ��� 1 �̸��̸�

    private int destPoint = 0;

    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if(agent != null)
        {
            agent.autoBraking = false; // ���������̰���������°�
            GotoNextPoint();
        }
    }


    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= remainDistanceMin)
        {
            // false�� �غ�Ȱ�. ����Ǹ�
            GotoNextPoint();
        }
    }
    private void GotoNextPoint()
    {
        if(patrolPoints.Length == 0)
        {
            Debug.LogError("1���ʿ�");
            enabled = false;
            return;
        }

        agent.destination = patrolPoints[destPoint].position;

        destPoint = (destPoint + 1) % patrolPoints.Length;
    }
}

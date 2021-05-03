using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // �ڵ����� NavMeshAgent�� ����
public class MoveAgent : MonoBehaviour
{
    public Transform wayPointGroup;
    private List<Transform> wayPoints = new List<Transform>(); // <>�� Generic. ���󿡼������ҿ���

    public int nextIndex = 0;
    private NavMeshAgent agent;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private bool _patrolling; // ��¥����
    public bool patrolling // �ۿ������� ����
    {
        get
        {
            return _patrolling; // ���⿡ ���������� ��¥������ �����־��ش�
        }
        set
        {
            _patrolling = value;
            if (_patrolling)
            {
                agent.speed = patrolSpeed;
                MoveWayPoint();
            }
        }
    }

    private Vector3 _traceTarget;
    public Vector3 traceTarget
    {
        get
        {
            return _traceTarget;
        }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            TraceTarget(_traceTarget);
        }
    }

    private void TraceTarget(Vector3 pos)
    {
        if (agent.isPathStale)
            return;
        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        _patrolling = false;
    }


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; // �������� ����������� �ӵ����̴¿ɼ� = false
    }

    // Start is called before the first frame update
    void Start()
    {
        wayPointGroup.GetComponentsInChildren<Transform>(wayPoints); // �ڽĵ��� Transform ����ͼ� ()�ȿ� �־���.. ()���� ����Ʈ��.
        wayPoints.RemoveAt(0); // 0��°�� �θ�� ����
        MoveWayPoint();
    }

    private void MoveWayPoint()
    {
        if (agent.isPathStale) // if(��ΰ� �غ�ȵ�������) return;
            return;
        agent.destination = wayPoints[nextIndex].position;
        agent.isStopped = false; // ������Ʈ on
    }

    // Update is called once per frame
    void Update()
    {
        if (!_patrolling)
            return;

        if(agent.velocity.sqrMagnitude >= 0.04f && agent.remainingDistance <= 0.5f) // sqr = ��Ʈ������� �Ÿ�. ��Ʈ�� �������� Ŀ�� ����
        {
            nextIndex = (++nextIndex) % wayPoints.Count; // �� 7���� 8�̵Ǵ¼��� 8%7=1. �ٽ� �������ϰԵ�
            MoveWayPoint();
        }
    }
}

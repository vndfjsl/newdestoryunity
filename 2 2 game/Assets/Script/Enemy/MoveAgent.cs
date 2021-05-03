using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))] // 자동으로 NavMeshAgent가 붙음
public class MoveAgent : MonoBehaviour
{
    public Transform wayPointGroup;
    private List<Transform> wayPoints = new List<Transform>(); // <>는 Generic. 영상에서설명할예정

    public int nextIndex = 0;
    private NavMeshAgent agent;

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;

    private bool _patrolling; // 진짜변수
    public bool patrolling // 밖에서보는 변수
    {
        get
        {
            return _patrolling; // 여기에 값을넣으면 진짜변수에 값을넣어준다
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
        agent.autoBraking = false; // 목적지에 가까워질수록 속도줄이는옵션 = false
    }

    // Start is called before the first frame update
    void Start()
    {
        wayPointGroup.GetComponentsInChildren<Transform>(wayPoints); // 자식들의 Transform 갖고와서 ()안에 넣어줌.. ()안이 리스트면.
        wayPoints.RemoveAt(0); // 0번째는 부모라서 삭제
        MoveWayPoint();
    }

    private void MoveWayPoint()
    {
        if (agent.isPathStale) // if(경로가 준비안돼있으면) return;
            return;
        agent.destination = wayPoints[nextIndex].position;
        agent.isStopped = false; // 에이전트 on
    }

    // Update is called once per frame
    void Update()
    {
        if (!_patrolling)
            return;

        if(agent.velocity.sqrMagnitude >= 0.04f && agent.remainingDistance <= 0.5f) // sqr = 루트연산없는 거리. 루트가 연산비용이 커서 제외
        {
            nextIndex = (++nextIndex) % wayPoints.Count; // 총 7개면 8이되는순간 8%7=1. 다시 루프를하게됨
            MoveWayPoint();
        }
    }
}

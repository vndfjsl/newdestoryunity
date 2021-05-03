using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        PATROL,
        TRACE,
        ATTACK,
        DIE
    }

    public EnemyState state = EnemyState.PATROL; // 초기값: PATROL
    private Transform playerTr; // 위치저장변수
    // 나중엔 GameManager로 불러옴

    public float attackDist = 5f;
    public float traceDist = 10f;
    public float judgeDelay = 0.3f; // 인공지능이 판단하는 시간. 매프레임마다돌리면 부하가너무커짐

    public bool isDie = false;
    private WaitForSeconds ws;
    private MoveAgent moveAgent;

   

    private void Awake()
    {
        moveAgent = GetComponent<MoveAgent>();
        ws = new WaitForSeconds(judgeDelay);
    }

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        // 플레이어 태그를 가진 게임오브젝트를 찾아서 가져온다.
        if(player != null)
        {
            playerTr = player.transform;
        }
    }

    // 내 상태를 체크하는 코루틴
    IEnumerator CheckState()
    {
        while(!isDie)
        {
            if(state == EnemyState.DIE)
            {
                yield break; // 코루틴 나가기
            }
            if(playerTr == null) // OnEnable이 Start보다빨라서 playerTr이 null이됨
            {
                yield return ws;
            }

            float distance = (playerTr.position - transform.position).sqrMagnitude;

            if(distance <= attackDist * attackDist) // sqr매그 쓰는중이라 이렇게해야댐
            {
                state = EnemyState.ATTACK;
            }
            else if(distance <= traceDist * traceDist)
            {
                state = EnemyState.TRACE;
            }
            else
            {
                state = EnemyState.PATROL;
            }
            yield return ws;
        }
    }

    // 상태를 기반으로 하는 액션 코루틴. 애니메이션제어
    IEnumerator Action()
    {
        while(!isDie)
        {
            yield return ws;
            switch(state)
            {
                case EnemyState.PATROL:
                    moveAgent.patrolling = true;
                    break;
                case EnemyState.TRACE:
                    moveAgent.traceTarget = playerTr.position;
                    break;
                case EnemyState.ATTACK:
                    moveAgent.Stop();
                    break;
                case EnemyState.DIE:
                    moveAgent.Stop(); // 이다음은 EnemyHealth에 맡김
                    break;
            }
        }
    }

    private void OnEnable() // 얘가 Start보다 빨라서
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
}

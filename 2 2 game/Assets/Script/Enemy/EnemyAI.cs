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

    public EnemyState state = EnemyState.PATROL; // �ʱⰪ: PATROL
    private Transform playerTr; // ��ġ���庯��
    // ���߿� GameManager�� �ҷ���

    public float attackDist = 5f;
    public float traceDist = 10f;
    public float judgeDelay = 0.3f; // �ΰ������� �Ǵ��ϴ� �ð�. �������Ӹ��ٵ����� ���ϰ��ʹ�Ŀ��

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
        // �÷��̾� �±׸� ���� ���ӿ�����Ʈ�� ã�Ƽ� �����´�.
        if(player != null)
        {
            playerTr = player.transform;
        }
    }

    // �� ���¸� üũ�ϴ� �ڷ�ƾ
    IEnumerator CheckState()
    {
        while(!isDie)
        {
            if(state == EnemyState.DIE)
            {
                yield break; // �ڷ�ƾ ������
            }
            if(playerTr == null) // OnEnable�� Start���ٻ��� playerTr�� null�̵�
            {
                yield return ws;
            }

            float distance = (playerTr.position - transform.position).sqrMagnitude;

            if(distance <= attackDist * attackDist) // sqr�ű� �������̶� �̷����ؾߴ�
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

    // ���¸� ������� �ϴ� �׼� �ڷ�ƾ. �ִϸ��̼�����
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
                    moveAgent.Stop(); // �̴����� EnemyHealth�� �ñ�
                    break;
            }
        }
    }

    private void OnEnable() // �갡 Start���� ����
    {
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
}

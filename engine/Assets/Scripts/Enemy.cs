using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public int currentX = 3;
    public int currentY = 1;
    public int hp = 100;
    public int mp = 100;
    public int armor = 0;

    public List<int> nextBehavior = new List<int>();
    public int behaviorIndex = 0;

    public void Start()
    {
        EnemySetBehavior(); // �� �������� ����(AI ���� �ʿ�.)
    }

    public void Move()
    {
        switch(nextBehavior[behaviorIndex])
        {
            case (int)Behavior.UP: // ��
                if (currentY > 0)
                {
                    currentY -= 1;
                }
                break;
            case (int)Behavior.DOWN: // �Ʒ�
                if (currentY < 2)
                {
                    currentY += 1;
                }
                break;
            case (int)Behavior.LEFT: // ��
                if (currentX > 0)
                {
                    currentX -= 1;
                }
                break;
            case (int)Behavior.RIGHT: // ��
                if (currentX < 3)
                {
                    currentX += 1;
                }
                break;
            case (int)Behavior.KnifeAttack:
                Debug.Log("Enemy Attack! Knife");
                StartCoroutine(MoveMap.Instance.ShowAttackCollision(Behavior.KnifeAttack, false));
                break;
            case (int)Behavior.Pike:
                Debug.Log("Enemy Attack! Pike");
                StartCoroutine(MoveMap.Instance.ShowAttackCollision(Behavior.Pike, false));
                break;
            case (int)Behavior.Shield:
                Debug.Log("Enemy Shield!");
                StartCoroutine(MoveMap.Instance.ShowAttackCollision(Behavior.Shield, false));
                break;
        }
        transform.DOMove(MoveMap.Instance.sliceMap[currentY, currentX].transform.position, 1f);
        behaviorIndex = (behaviorIndex + 1) % 3;
    }

    public void EnemySetBehavior()
    {
        nextBehavior.Clear();

        for (int i=0; i<3; i++)
        {
            int behaviorType = Random.Range(0, 7);
            if (currentY <= 0 && behaviorType == (int)Behavior.UP) // ���ʳ��̸�
                behaviorType = (int)Behavior.DOWN;
            else if (currentY >= 2 && behaviorType == (int)Behavior.DOWN)
                behaviorType = (int)Behavior.UP;
            if (currentX <= 0 && behaviorType == (int)Behavior.LEFT) // ���ʳ��̸�
                behaviorType = (int)Behavior.RIGHT;
            else if (currentX >= 3 && behaviorType == (int)Behavior.RIGHT)
                behaviorType = (int)Behavior.LEFT;

            nextBehavior.Add(behaviorType);
        }
    }
}

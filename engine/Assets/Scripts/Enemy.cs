using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public int currentX = 3;
    public int currentY = 1;
    public int hp = 150;
    public int mp = 100;
    public int armor = 0;

    public List<int> nextBehavior = new List<int>();
    public int behaviorIndex = 0;
    public bool specialAttack1able = true;

    public void Start()
    {
        // EnemySetBehavior(); // 적 다음공격 설정(AI 삽입 필요.)
    }

    public void Move()
    {
        switch(nextBehavior[behaviorIndex])
        {
            case (int)Behavior.UP: // 위
                if (currentY > 0)
                {
                    currentY -= 1;
                }
                break;
            case (int)Behavior.DOWN: // 아래
                if (currentY < 2)
                {
                    currentY += 1;
                }
                break;
            case (int)Behavior.LEFT: // 왼
                if (currentX > 0)
                {
                    currentX -= 1;
                }
                break;
            case (int)Behavior.RIGHT: // 오
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
            case (int)Behavior.Spear:
                Debug.Log("Enemy SpecialSpear!");
                StartCoroutine(MoveMap.Instance.ShowAttackCollision(Behavior.Spear, false));
                break;
        }
        transform.DOMove(MoveMap.Instance.sliceMap[currentY, currentX].transform.position + new Vector3(0.5f, 0, 0), 1f);
        behaviorIndex = (behaviorIndex + 1) % 3;
    }

    public void PushOut()
    {
        currentX += 1;
        transform.DOMove(MoveMap.Instance.sliceMap[currentY, currentX].transform.position + new Vector3(0.5f, 0, 0), 1f);
    }

    public void EnemySetBehavior()
    {
        // nextBehavior.Clear();

        //for (int i=0; i<3; i++)
        //{
            List<int> bhArr = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
        if (currentY <= 0)
        {
            bhArr.Remove((int)Behavior.UP);
        }
        else if (currentY >= 2)
        {
            bhArr.Remove((int)Behavior.DOWN);
        }

        if (currentX <= 0)
        {
            bhArr.Remove((int)Behavior.LEFT);
        }
        else if (currentX >= 3)
        {
            bhArr.Remove((int)Behavior.RIGHT);
        }

        if (!specialAttack1able)
        {
            bhArr.Remove((int)Behavior.Spear);
        }

        if (mp < 10)
        {
            bhArr.Remove((int)Behavior.KnifeAttack);
        }

        if (mp < 30)
        {
            bhArr.Remove((int)Behavior.Pike);
        }

        int randBhIndex = Random.Range(0, bhArr.Count);
        nextBehavior.Add(bhArr[randBhIndex]);
    }
}

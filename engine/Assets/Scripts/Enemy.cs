using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentX = 3;
    public int currentY = 1;
    public int hp = 100;
    public int mp = 100;

    public List<int> nextBehavior = new List<int>();
    public int behaviorIndex = 0;

    public Transform attack1Trm;

    //public int[] enemyAttackCollisions; // 플레이어 공격스킬범위. 123456789 순서대로
    /*
     * 123
     * 456
     * 789
     * 0b 010 111 010 = 십자가
     * = 010
     *   111
     *   010
     * 
     */

    public void Start()
    {
        SetAttackCollision();
        EnemySetBehavior();
    }

    public void SetAttackCollision()
    {
        //enemyAttackCollisions[0] = 0b000011000; // 000 011 000, 정면공격
        //enemyAttackCollisions[1] = 0b010010000; // 010 010 000, 위공격
        //enemyAttackCollisions[2] = 0b000010111; // 000 010 111, 아래모두
        //enemyAttackCollisions[3] = 0b110110110; // 110 110 110, 전방휩쓸기
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
                Debug.Log("적의 공격");
                MoveMap.Instance.AttackProcess((int)Behavior.KnifeAttack, false);
                break;
        }
        behaviorIndex = (behaviorIndex + 1) % 3;
    }

    public void EnemySetBehavior()
    {
        nextBehavior.Clear();

        for (int i=0; i<3; i++)
        {
            int behaviorType = Random.Range(0, 5); // 0~4
            switch(behaviorType)
            {
                case 0:
                    nextBehavior.Add((int)Behavior.UP);
                    break;

                case 1:
                    nextBehavior.Add((int)Behavior.LEFT);
                    break;

                case 2:
                    nextBehavior.Add((int)Behavior.DOWN);
                    break;

                case 3:
                    nextBehavior.Add((int)Behavior.RIGHT);
                    break;
                case 4:
                    
                    nextBehavior.Add((int)Behavior.KnifeAttack);
                    break;
            }
        }
    }
}

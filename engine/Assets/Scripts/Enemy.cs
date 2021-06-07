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

    //public int[] enemyAttackCollisions; // �÷��̾� ���ݽ�ų����. 123456789 �������
    /*
     * 123
     * 456
     * 789
     * 0b 010 111 010 = ���ڰ�
     * = 010
     *   111
     *   010
     * 
     */

    public void Start()
    {
        SetAttackCollision();
        SetBehavior(new char[6] {'w','a','s','s','a','d' });
    }

    public void SetAttackCollision()
    {
        //enemyAttackCollisions[0] = 0b000011000; // 000 011 000, �������
        //enemyAttackCollisions[1] = 0b010010000; // 010 010 000, ������
        //enemyAttackCollisions[2] = 0b000010111; // 000 010 111, �Ʒ����
        //enemyAttackCollisions[3] = 0b110110110; // 110 110 110, �����۾���
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
                MoveMap.Instance.AttackProcess((int)Behavior.KnifeAttack, false);
                break;
        }
        behaviorIndex++;
    }

    public void SetBehavior(char[] bhArr)
    {
        for(int i=0; i<bhArr.Length; i++)
        {
            switch(bhArr[i])
            {
                case 'w':
                    nextBehavior.Add((int)Behavior.UP);
                    break;

                case 'a':
                    nextBehavior.Add((int)Behavior.LEFT);
                    break;

                case 's':
                    nextBehavior.Add((int)Behavior.DOWN);
                    break;

                case 'd':
                    nextBehavior.Add((int)Behavior.RIGHT);
                    break;
                case 'K':
                    nextBehavior.Add((int)Behavior.KnifeAttack);
                    break;
            }
        }
    }
}

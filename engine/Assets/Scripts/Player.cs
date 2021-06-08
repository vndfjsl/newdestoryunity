using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentX = 0;
    public int currentY = 1;
    public int hp = 100;
    public int mp = 100;

    public List<int> nextBehavior = new List<int>();
    public int behaviorIndex = 0;

    public Transform attack1Trm;

    //public int[] playerAttackCollisions; // �÷��̾� ���ݽ�ų����. 123456789 �������
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

    //private void Start()
    //{
    //    SetAttackCollision();
    //}

    //public void SetAttackCollision()
    //{
    //    playerAttackCollisions[0] = 0b000011000; // 000 011 000, �������
    //    playerAttackCollisions[1] = 0b111111111; // 111 111 111, ��ü����
    //    playerAttackCollisions[2] = 0b000101000; // 010 101 000, �¿����
    //    playerAttackCollisions[3] = 0b011011011; // 011 011 011, �����۾���
    //}

    public void Move()
    {
        switch (nextBehavior[behaviorIndex])
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
                MoveMap.Instance.AttackProcess((int)Behavior.KnifeAttack, true); // �ڿ�true�� �÷��̾������ƴ����˻�
                break;
        }
        behaviorIndex = (behaviorIndex + 1) % 3;

        // hp -= 20;
    }

    public void InitBehavior()
    {
        nextBehavior.Clear();
    }
}

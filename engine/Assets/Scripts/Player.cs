using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public int currentX = 0;
    public int currentY = 1;
    public int hp = 100;
    public int mp = 100;
    public int armor = 0;

    public List<int> nextBehavior = new List<int>();
    public int behaviorIndex = 0;

    

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
                StartCoroutine(GameManager.Instance.ShowAttackCollision(Behavior.KnifeAttack, true)); // �ڿ�true�� �÷��̾������ƴ����˻�
                break;
            case (int)Behavior.Pike:
                StartCoroutine(GameManager.Instance.ShowAttackCollision(Behavior.Pike, true));
                break;
            case (int)Behavior.Shield:
                StartCoroutine(GameManager.Instance.ShowAttackCollision(Behavior.Shield, true));
                break;
            case (int)Behavior.Spear:
                StartCoroutine(GameManager.Instance.ShowAttackCollision(Behavior.Spear, true));
                break;
        }
        // transform.position = MoveMap.Instance.sliceMap[currentY, currentX].transform.position;
        transform.DOMove(GameManager.Instance.sliceMap[currentY, currentX].transform.position - new Vector3(0.5f,0,0), 1f);
        behaviorIndex = (behaviorIndex + 1) % 3;
        // hp -= 20;
    }

    public void PushOut()
    {
        currentX -= 1;
        transform.DOMove(GameManager.Instance.sliceMap[currentY, currentX].transform.position - new Vector3(0.5f, 0, 0), 1f);
    }

    public void InitBehavior()
    {
        nextBehavior.Clear();
    }
}

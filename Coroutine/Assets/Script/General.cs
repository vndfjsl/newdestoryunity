using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buf
{
    None = 0,
    Heist = 1, // 2^0
    AttackDouble = 2, // 2^1
    HPRegen = 4 // 2^2
    // BadSample = 5   // �̰Ǿȵ�. BadSample = Heist + HPRegen�̴� �浹��
}

public class General : MonoBehaviour
{
    private Buf bufStat;
    void Start()
    {
        bufStat |= Buf.Heist; // 0000���� 0001�� ��.
        bufStat |= Buf.HPRegen; // 0001���� 0101�� ��.

        if((bufStat & Buf.Heist) == Buf.Heist) // 0101 & 0001 = 0001 = HeistO
        {
            Debug.Log("���̽�Ʈ ����");
        }
        if ((bufStat & Buf.AttackDouble) == Buf.AttackDouble)
        {
            Debug.Log("������� ����");
        }
        if ((bufStat & Buf.HPRegen) == Buf.HPRegen)
        {
            Debug.Log("ü����� ����");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

        //int a = 23002311;
        //int b = 6;
        //int c = a & b;
        //Debug.Log(c); // �ɼ� �����Ҷ� ��Ʈ�� ���̾�. WINAPI���� ����.
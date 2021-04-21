using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buf
{
    None = 0,
    Heist = 1, // 2^0
    AttackDouble = 2, // 2^1
    HPRegen = 4 // 2^2
    // BadSample = 5   // 이건안됨. BadSample = Heist + HPRegen이니 충돌남
}

public class General : MonoBehaviour
{
    private Buf bufStat;
    void Start()
    {
        bufStat |= Buf.Heist; // 0000에서 0001이 됨.
        bufStat |= Buf.HPRegen; // 0001에서 0101이 됨.

        if((bufStat & Buf.Heist) == Buf.Heist) // 0101 & 0001 = 0001 = HeistO
        {
            Debug.Log("헤이스트 상태");
        }
        if ((bufStat & Buf.AttackDouble) == Buf.AttackDouble)
        {
            Debug.Log("더블어택 상태");
        }
        if ((bufStat & Buf.HPRegen) == Buf.HPRegen)
        {
            Debug.Log("체력재생 상태");
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
        //Debug.Log(c); // 옵션 설정할때 비트를 많이씀. WINAPI에서 쓰듯.
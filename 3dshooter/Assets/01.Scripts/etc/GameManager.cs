using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    public Transform playerTR;

    void Awake()
    {
        if (instance != null)
            Debug.LogError("�ټ��� ���ӸŴ����� �������Դϴ�.");
        instance = this;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

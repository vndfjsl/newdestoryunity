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
            Debug.LogError("다수의 게임매니저가 실행중입니다.");
        instance = this;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

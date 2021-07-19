using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefSaveSystem : MonoBehaviour
{
    const string SaveHpKey = "CurHp";

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.SetInt("CurHp", 100);
            Debug.Log("현재 체력 저장 : " + PlayerPrefs.GetInt("CurHp"));
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("현재 체력 로드 : " + PlayerPrefs.GetInt("CurHp"));
        }
    }
}

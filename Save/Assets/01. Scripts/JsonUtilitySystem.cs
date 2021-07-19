using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonUtilitySystem : MonoBehaviour
{
    [SerializeField] const string saveFileName = "jsonUtility.sav"; // 확장자 상관 x
    [SerializeField] private string name = "zero7";
    [SerializeField] [HideInInspector] private int level = 13;
    // SerializeField 용도 1. 인스펙터 디스플레이. 2. 저장 가능
    // HideInInSpector 용도 : 인스펙터에서 가림(기획자한테 보여주기싫은거)

    [System.NonSerialized]
    public int age = 100; // 얘만 저장이 안됨ㅋㅋ

    public Transform myTrm; // 근데이거도 저장이 안됨ㅋㅋ 잘하면할수있음

    public string[] friends;

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Save : " + GetFilePath(saveFileName)); // 여기다가 저장할게

            string jsonString = JsonUtility.ToJson(this);
            StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            sw.WriteLine(jsonString);
            sw.Close();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // 여기에서 로드할게

            string fileString = GetFilePath(saveFileName);
            if(File.Exists(fileString))
            {
                StreamReader sr = new StreamReader(fileString);
                string jsonString = sr.ReadToEnd();

                JsonUtility.FromJsonOverwrite(jsonString, this);

                print(jsonString);
            }
            else
            {
                print("파일없어요");
            }
        }
    }
}

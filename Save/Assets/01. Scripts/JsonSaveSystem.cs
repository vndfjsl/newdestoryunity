using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSaveSystem : MonoBehaviour
{
    const string saveFileName = "Mirror.sav"; // 확장자 상관 x
    private string name = "그들만의이름";
    private int level = 13;

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Save to : " + GetFilePath(saveFileName)); // 여기다가 저장할게

            StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            sw.WriteLine(name);
            sw.WriteLine(level);
            sw.Close(); // 절대빼먹으면안댐
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // 여기에서 로드할게

            StreamReader sr = new StreamReader(GetFilePath(saveFileName));

            print("1번째 데이터 이름 : " + sr.ReadLine());
            print("2번째 데이터 레벨 : " + sr.ReadLine());

            sr.Close();
        }
    }
}

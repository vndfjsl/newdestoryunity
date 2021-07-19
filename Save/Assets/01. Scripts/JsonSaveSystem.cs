using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonSaveSystem : MonoBehaviour
{
    const string saveFileName = "Mirror.sav"; // Ȯ���� ��� x
    private string name = "�׵鸸���̸�";
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
            print("Save to : " + GetFilePath(saveFileName)); // ����ٰ� �����Ұ�

            StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            sw.WriteLine(name);
            sw.WriteLine(level);
            sw.Close(); // ���뻩������ȴ�
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // ���⿡�� �ε��Ұ�

            StreamReader sr = new StreamReader(GetFilePath(saveFileName));

            print("1��° ������ �̸� : " + sr.ReadLine());
            print("2��° ������ ���� : " + sr.ReadLine());

            sr.Close();
        }
    }
}

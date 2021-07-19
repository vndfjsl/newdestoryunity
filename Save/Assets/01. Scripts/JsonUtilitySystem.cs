using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonUtilitySystem : MonoBehaviour
{
    [SerializeField] const string saveFileName = "jsonUtility.sav"; // Ȯ���� ��� x
    [SerializeField] private string name = "zero7";
    [SerializeField] [HideInInspector] private int level = 13;
    // SerializeField �뵵 1. �ν����� ���÷���. 2. ���� ����
    // HideInInSpector �뵵 : �ν����Ϳ��� ����(��ȹ������ �����ֱ������)

    [System.NonSerialized]
    public int age = 100; // �길 ������ �ȵʤ���

    public Transform myTrm; // �ٵ��̰ŵ� ������ �ȵʤ��� ���ϸ��Ҽ�����

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
            print("Save : " + GetFilePath(saveFileName)); // ����ٰ� �����Ұ�

            string jsonString = JsonUtility.ToJson(this);
            StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            sw.WriteLine(jsonString);
            sw.Close();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // ���⿡�� �ε��Ұ�

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
                print("���Ͼ����");
            }
        }
    }
}

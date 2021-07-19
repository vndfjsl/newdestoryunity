using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BinaryFormat : MonoBehaviour
{
    const string saveFileName = "eight.sav"; // Ȯ���� ��� x
    private string name = "777";
    private int level = 13;

    [System.Serializable] // Ŭ���� ����ȭ���ִ°�(�����鿡����)
    private class DataContainer //Ŭ���� �� Ŭ����
    {
        public DataContainer(string name, int level)
        {
            _name = name;
            _level = level;
        }

        public string _name;
        public int _level;
    }

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Save : " + GetFilePath(saveFileName)); // ����ٰ� �����Ұ�

            DataContainer dc = new DataContainer(name, level);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(GetFilePath(saveFileName), FileMode.OpenOrCreate);

            bf.Serialize(fs, dc);
            fs.Close();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // ���⿡�� �ε��Ұ�

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(GetFilePath(saveFileName), FileMode.Open);
            DataContainer dc = bf.Deserialize(fs) as DataContainer;

            print("name : " + dc._name);
            print("level : " + dc._level);

            fs.Close();
        }
    }
}

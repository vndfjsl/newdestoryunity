using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BinarySaveSystem : MonoBehaviour
{
    const string saveFileName = "seven.sav"; // Ȯ���� ��� x
    private string name = "�׵鸸�ǰ���";
    private int level = 13;

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Save to : " + GetFilePath(saveFileName)); // ����ٰ� �����Ұ�

            FileStream fs = new FileStream(GetFilePath(saveFileName), FileMode.OpenOrCreate);
            // FileMode = ���� ����ұ�? OpenOrCreate = �����鸸���

            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(name);
            bw.Write(level);

            fs.Close();
            bw.Close(); // �� �ݾ���� �Ѵ�.
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // ���⿡�� �ε��Ұ�

            FileStream fs = new FileStream(GetFilePath(saveFileName), FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            print(br.ReadString());
            print(br.ReadInt32());

            fs.Close();
            br.Close();
        }
    }
}

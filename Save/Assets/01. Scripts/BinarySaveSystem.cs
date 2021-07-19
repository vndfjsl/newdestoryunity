using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BinarySaveSystem : MonoBehaviour
{
    const string saveFileName = "seven.sav"; // 확장자 상관 x
    private string name = "그들만의게임";
    private int level = 13;

    private string GetFilePath(string fileName)
    {
        return Application.persistentDataPath + "/" + fileName;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            print("Save to : " + GetFilePath(saveFileName)); // 여기다가 저장할게

            FileStream fs = new FileStream(GetFilePath(saveFileName), FileMode.OpenOrCreate);
            // FileMode = 열때 어떻게할까? OpenOrCreate = 없으면만들어

            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(name);
            bw.Write(level);

            fs.Close();
            bw.Close(); // 꼭 닫아줘야 한다.
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // 여기에서 로드할게

            FileStream fs = new FileStream(GetFilePath(saveFileName), FileMode.Open);
            BinaryReader br = new BinaryReader(fs);

            print(br.ReadString());
            print(br.ReadInt32());

            fs.Close();
            br.Close();
        }
    }
}

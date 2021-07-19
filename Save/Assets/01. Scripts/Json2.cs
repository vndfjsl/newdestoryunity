using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq; // �ܺ� �÷�����
using System.IO;

public class Json2 : MonoBehaviour
{
    const string saveFileName = "json.sav"; // Ȯ���� ��� x
    private string name = "02";
    private int level = 13;

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

            // JSON �����Ͱ���
            JObject jObj = new JObject();
            jObj.Add("ComponentName", GetType().ToString());

            JObject jDataObject = new JObject(); // ��ü �ϳ���
            jObj.Add("Data", jDataObject);

            jDataObject.Add("Name", name);
            jDataObject.Add("Level", level);

            JArray jFriendsArray = JArray.FromObject(friends);
            jDataObject.Add("Friends", jFriendsArray);

            // ������ ����
            StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            sw.WriteLine(jObj.ToString());
            sw.Close();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // ���⿡�� �ε��Ұ�

            StreamReader sr = new StreamReader(GetFilePath(saveFileName));
            string jsonString = sr.ReadToEnd();
            sr.Close();

            print(jsonString);

            // ���� String�� JObject�� �ٲ�
            JObject jObj = JObject.Parse(jsonString);

            name = jObj["Data"]["Name"].Value<string>();
            level = jObj["Data"]["Level"].Value<int>();
            friends = jObj["Data"]["Friends"].ToObject<string[]>();
        }
    }
}

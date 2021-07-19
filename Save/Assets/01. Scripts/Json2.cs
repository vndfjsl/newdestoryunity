using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq; // 외부 플러그인
using System.IO;

public class Json2 : MonoBehaviour
{
    const string saveFileName = "json.sav"; // 확장자 상관 x
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
            print("Save : " + GetFilePath(saveFileName)); // 여기다가 저장할게

            // JSON 데이터가공
            JObject jObj = new JObject();
            jObj.Add("ComponentName", GetType().ToString());

            JObject jDataObject = new JObject(); // 객체 하나더
            jObj.Add("Data", jDataObject);

            jDataObject.Add("Name", name);
            jDataObject.Add("Level", level);

            JArray jFriendsArray = JArray.FromObject(friends);
            jDataObject.Add("Friends", jFriendsArray);

            // 실제로 저장
            StreamWriter sw = new StreamWriter(GetFilePath(saveFileName));
            sw.WriteLine(jObj.ToString());
            sw.Close();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            print("Load to : " + GetFilePath(saveFileName)); // 여기에서 로드할게

            StreamReader sr = new StreamReader(GetFilePath(saveFileName));
            string jsonString = sr.ReadToEnd();
            sr.Close();

            print(jsonString);

            // 읽은 String을 JObject로 바꿔
            JObject jObj = JObject.Parse(jsonString);

            name = jObj["Data"]["Name"].Value<string>();
            level = jObj["Data"]["Level"].Value<int>();
            friends = jObj["Data"]["Friends"].ToObject<string[]>();
        }
    }
}

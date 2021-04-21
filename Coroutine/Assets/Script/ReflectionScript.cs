using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ReflectionScript : MonoBehaviour
{
    public string className = "Player";
    void Start()
    {
        Type t = Type.GetType(className);

        Player p = (Player)Activator.CreateInstance(t);


        //Player a = new Player();

        //Type t = a.GetType();
        //FieldInfo[] fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static); // System.Reflection ¾îÂ¼°í ÀúÂ¼°í

        //foreach(FieldInfo f in fields)
        //{
        //    Debug.Log(f.FieldType.Name + " : " + f.Name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

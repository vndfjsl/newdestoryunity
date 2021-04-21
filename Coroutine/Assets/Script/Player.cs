using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string name;
    private int age;

    public void Move()
    {
        Debug.Log("움직이면 " + name + "은 사라져");
    }
    private void Die()
    {

    }
}

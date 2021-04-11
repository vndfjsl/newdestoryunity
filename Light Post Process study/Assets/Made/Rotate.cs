using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Transform target;
    public float speed = 40;

    private float sum;

    void Update()
    {
        float y = speed * Time.deltaTime;
        target.Rotate(new Vector3(0,y,0),Space.World);
        sum += y;
        if (sum >= 360 || sum <= -360)
        {
            sum = 0;
            speed *= -1;
            
        }
    }
}

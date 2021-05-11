using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmoos : MonoBehaviour
{
    public Color c = Color.blue;
    public float radius = 0.3f;

    private void OnDrawGizmos()
    {
        Gizmos.color = c;
        Gizmos.DrawSphere(transform.position, radius);
    }
}

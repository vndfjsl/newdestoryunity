using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    public Color color = Color.red;
    public float radius = 0.4f;

    private void OnDrawGizMos(int layerIndex)
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

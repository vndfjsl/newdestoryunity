using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    NavMeshAgent agent;
    public LayerMask whatIsGround;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float depth = Camera.main.farClipPlane; // 클리핑 far 그거말함 
            RaycastHit hit;
            if(Physics.Raycast(ray.origin,ray.direction, out hit, depth, whatIsGround))
            {
                agent.destination = hit.point;
            }
        }
    }
}

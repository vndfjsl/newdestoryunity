using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAgent : MonoBehaviour
{
    [SerializeField]
    Transform goalTransform;

    void Start()
    {
        NavMeshAgent playerAgent = GetComponent<NavMeshAgent>();
        if (playerAgent != null)
            playerAgent.destination = goalTransform.position;
    }
}

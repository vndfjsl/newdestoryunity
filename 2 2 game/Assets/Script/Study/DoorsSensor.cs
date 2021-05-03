using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsSensor : MonoBehaviour
{
    public float openTime = 2f;
    public float openSpeed = 5f;
    public Vector3 openDistance = new Vector3(4, 0, 0);

    private bool isOpen = false;
    private Vector3 originPoint;
    private Vector3 targetPosition = Vector3.zero;
    private Coroutine co = null;

    private void Awake()
    {
        originPoint = transform.parent.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(isOpen && co != null)
            {
                StopCoroutine(StayOpen());
            }
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(StayOpen());
        }
    }

    private void OpenDoor()
    {
        if (isOpen)
            return;
        targetPosition = originPoint + openDistance;
        isOpen = true;
    }

    private void CloseDoor()
    {
        isOpen = false;
    }

    private void Update()
    {
        if (isOpen)
        {
            Vector3 nextPos = Vector3.Lerp
                (transform.parent.position,
                targetPosition,
                Time.deltaTime * openSpeed);
            transform.parent.position = nextPos;
        }
        else
        {
            if((transform.parent.position - originPoint).sqrMagnitude >= 0.01f)
            {
                Vector3 nextPos = Vector3.Lerp
                    (transform.parent.position,
                    originPoint,
                    Time.deltaTime * openSpeed);
                transform.parent.position = nextPos;
            }
        }
    }

    IEnumerator StayOpen()
    {
        yield return new WaitForSeconds(openTime);
        CloseDoor();
    }
}

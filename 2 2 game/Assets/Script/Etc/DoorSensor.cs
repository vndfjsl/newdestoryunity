using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSensor : MonoBehaviour
{
    public float openTime = 2f;
    public float openSpeed = 5f;
    public Vector3 openDistance = new Vector3(4f,0f,0f);
    private bool isOpen = false;
    private Vector3 originPoint;
    private Vector3 targetPosition = Vector3.zero;
    public GameObject parentDoor;

    private void Awake()
    {
        originPoint = parentDoor.transform.position;
    }

    private void Update()
    {
        if (isOpen) // ��������
        {
            Vector3 nextPos = Vector3.Lerp(parentDoor.transform.position, targetPosition, Time.deltaTime * openSpeed);
            parentDoor.transform.position = nextPos;
        }
        else // ��������
        {
            if ((parentDoor.transform.position - originPoint).sqrMagnitude >= 0.01f) // magnitude���� �����ٿ����� �� �Լ�. �������� ���ϰ� ���ؼ� �Ÿ����Ҷ��� �̰ž��°Գ���
            {
                Vector3 nextPos = Vector3.Lerp(parentDoor.transform.position, originPoint, Time.deltaTime * openSpeed);
                parentDoor.transform.position = nextPos;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(isOpen)
            {
                StopCoroutine("StayOpen"); // ������������������ ó���ϳ���������.
            }
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine("StayOpen"); // StartCoroutine(StayOpen())�� ��. ������ ""�� �Լ����ڵ� �ϳ��ۿ����������� ������ ""�ۿ��ȵ���.
        }
    }

    private void OpenDoor()
    {
        if (isOpen)
            return;
        targetPosition = originPoint + openDistance;
        isOpen = true;
    }

    private IEnumerator StayOpen()
    {
        yield return new WaitForSeconds(openTime);
        CloseDoor();
    }

    private void CloseDoor()
    {
        isOpen = false;
    }
}

// gameObject.tag ���� string�� �����ؼ� ���������ڿ��� ����. �׷��ϱ� �� �����ϰ� ���α����� c++�εǾ������� �� CompareTag������ϴ°� �̷Ӵ�
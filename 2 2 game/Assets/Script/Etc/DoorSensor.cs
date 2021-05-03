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
        if (isOpen) // 열리는중
        {
            Vector3 nextPos = Vector3.Lerp(parentDoor.transform.position, targetPosition, Time.deltaTime * openSpeed);
            parentDoor.transform.position = nextPos;
        }
        else // 닫히는중
        {
            if ((parentDoor.transform.position - originPoint).sqrMagnitude >= 0.01f) // magnitude에서 제곱근연산을 뺀 함수. 제곱근이 부하가 심해서 거리비교할때는 이거쓰는게나음
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
                StopCoroutine("StayOpen"); // 여러개돌리고있으면 처음하나만정지됨.
            }
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            StartCoroutine("StayOpen"); // StartCoroutine(StayOpen())도 됨. 심지어 ""는 함수인자도 하나밖에못보내지만 옛날엔 ""밖에안됐음.
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

// gameObject.tag 쓰면 string을 선언해서 내가쓴문자열과 비교함. 그러니까 좀 복잡하고 내부구현은 c++로되어있으니 걍 CompareTag를사용하는게 이롭다
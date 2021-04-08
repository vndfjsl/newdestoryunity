using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code.gondr.net => 87�� ���� 
public class PlayersInput : MonoBehaviour
{
    public string frontAxisName = "Vertical";
    public string rightAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";

    public float frontMove { get; private set; }
    public float rightMove { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public Vector3 mousePos { get; private set; }
    public LayerMask whatIsGround;

    void Update()
    {
        frontMove = Input.GetAxis(frontAxisName);
        rightMove = Input.GetAxis(rightAxisName);
        fire = Input.GetButtonDown(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //mousePosition�� ��ũ����ǥ�� ���´�. 
        //���� ��ũ����ǥ�� ���̸� ��� �� ���⿡ �ִ� ������ǥ�� ���� �� �ִ�.
        RaycastHit hit;
        float depth = Camera.main.farClipPlane; //f12������ ����Ƽ �Լ����� ���� ������ �� �� �ִ�.
        if (Physics.Raycast(camRay, out hit, depth, whatIsGround))
        {
            mousePos = hit.point;
            // Debug.DrawRay(Camera.main.transform.position, 
            //             camRay.direction * depth,
            //             Color.red,
            //             0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, 0.5f);
        //���콺 ��ġ�� 0.5���������� ���� �׷��ش�.
    }
}

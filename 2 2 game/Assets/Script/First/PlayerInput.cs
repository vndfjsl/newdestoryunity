using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string frontAxisName = "Vertical";
    public string rightAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public float frontMove { get; private set; }
    public float rightMove { get; private set; }
    public bool fire       { get; private set; }
    public bool reload     { get; private set; }
    public Vector3 mousePos { get; private set; }
    public LayerMask whatIsGround;

    void Update()
    {
        frontMove = Input.GetAxis(frontAxisName);
        rightMove = Input.GetAxis(rightAxisName);
        fire = Input.GetButtonDown(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float depth = Camera.main.farClipPlane;
        if (Physics.Raycast(cameraRay, out hit, depth, whatIsGround))
        {
            mousePos = hit.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(mousePos, 0.5f);
    }

    // 좌표계3개: world(전체) screen(해상도) viewport(화면을 0~1로 만드는 비율)
    // 게임을 잘만들고싶으면 게임을 하지마라.
}

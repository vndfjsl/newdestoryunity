using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 6f;
    public float gravity = -20f;

    private PlayersInput playersInput;
    private CharacterController characterController;

    private void Awake()
    {
        playersInput = GetComponent<PlayersInput>();
        characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void Move()
    {
        Vector3 dir = (transform.forward * playersInput.frontMove
                        + transform.right * playersInput.rightMove).normalized * moveSpeed;

        dir.y = gravity;
        characterController.Move(dir * Time.deltaTime);
    }

    private void Rotate()
    {
        Vector3 target = playersInput.mousePos;
        target.y = 0;
        Vector3 v = target - transform.position;
        float degree = Mathf.Atan2(v.x, v.z) * Mathf.Rad2Deg;
        float rot = Mathf.LerpAngle(
                        transform.eulerAngles.y,
                        degree,
                        Time.deltaTime * rotateSpeed);
        transform.eulerAngles = new Vector3(0, rot, 0);
    }
}
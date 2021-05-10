using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveMent : MonoBehaviour
{
    Vector3 moveDir = Vector3.zero;
    CharacterController cc;

    public float moveSpeed;
    public float jumpSpeed;
    public float gravity;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV();
    }

    // Update is called once per frame
    void Update()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveDir.z = Input.GetAxisRaw("Vertical") * moveSpeed;

        if(cc.isGrounded && Input.GetButton("Jump"))
        {
            moveDir.y = jumpSpeed;
        }

        moveDir.y -= gravity * Time.deltaTime;

        cc.Move(moveDir * Time.deltaTime);
    }
}

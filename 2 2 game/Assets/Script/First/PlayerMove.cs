using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rigid;
#region 플레이어이동
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        rigid.velocity = transform.forward.normalized * z * speed;
        rigid.rotation = rigid.rotation * Quaternion.Euler(0, x, 0);
    }
#endregion
}

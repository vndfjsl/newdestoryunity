using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public Gun gun;
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (playerInput.fire)
        {
            gun.Fire();
        }
        if (playerInput.reload)
        {
            if (gun.Reload())
            {
                //���⼭ �ִϸ��̼� ���
            }
        }
    }
}

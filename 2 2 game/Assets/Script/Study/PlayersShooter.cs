using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersShooter : MonoBehaviour
{
    public PlayersGun playersGun;
    private PlayersInput playersInput;

    private void Awake()
    {
        playersInput = GetComponent<PlayersInput>();
    }

    private void Update()
    {
        if (playersInput.fire)
        {
            playersGun.Fire();
        }
        if (playersInput.reload)
        {
            if (playersGun.Reload())
            {
                //여기서 애니메이션 재생
            }
        }
    }
}

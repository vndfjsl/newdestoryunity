using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}


public class PlayerAnimation : MonoBehaviour
{

    public PlayerAnim playerAnim;
    [HideInInspector]
    public Animation animation;
    private PlayerInput playerInput;

    private void Awake()
    {
        animation = GetComponent<Animation>();
        playerInput = GetComponent<PlayerInput>();
    }


    // Start is called before the first frame update
    void Start()
    {
        animation.clip = playerAnim.idle;
        animation.Play();
    }

    // Update is called once per frame
    void Update()
    {
        float v = playerInput.frontMove;
        float h = playerInput.rightMove;

        if (v >= 0.1f)
        {
            animation.CrossFade(playerAnim.runF.name, 0.3f);
        }
        else if (v <= -0.1f)
        {
            animation.CrossFade(playerAnim.runB.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            animation.CrossFade(playerAnim.runR.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            animation.CrossFade(playerAnim.runL.name, 0.3f);
        }
        else
        {
            animation.CrossFade(playerAnim.idle.name, 0.3f);
        }
    }
}

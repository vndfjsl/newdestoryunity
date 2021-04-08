using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayersAnim
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
}

public class PlayersAnimation : MonoBehaviour
{
    public PlayersAnim playersAnim;

    [HideInInspector]
    public Animation animation;
    private PlayersInput playersInput;

    private void Awake()
    {
        animation = GetComponent<Animation>();
        playersInput = GetComponent<PlayersInput>();
    }

    private void Start()
    {
        animation.clip = playersAnim.idle;
        animation.Play();
    }

    private void Update()
    {
        float v = playersInput.frontMove;
        float h = playersInput.rightMove;

        if (v >= 0.1f)
        {
            animation.CrossFade(playersAnim.runF.name, 0.3f);
        }
        else if (v <= -0.1f)
        {
            animation.CrossFade(playersAnim.runB.name, 0.3f);
        }
        else if (h >= 0.1f)
        {
            animation.CrossFade(playersAnim.runR.name, 0.3f);
        }
        else if (h <= -0.1f)
        {
            animation.CrossFade(playersAnim.runL.name, 0.3f);
        }
        else
        {
            animation.CrossFade(playersAnim.idle.name, 0.3f);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public enum enFadeState
{
    None,
    FadeOut,
    ChangeBackg,
    FadeIn,
    Done
}

public class FadeCo : MonoBehaviour
{
    enFadeState fadeState = enFadeState.None;

    public int fadeOutDelay = 3;
    public int fadeInDelay = 5;
    public Image backImage;

    IEnumerator iStateCo = null;

    private void Awake()
    {
        backImage = this.gameObject.GetComponent<Image>();
        if(backImage == null)
        {
            Debug.Log("background image is null");
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && fadeState == enFadeState.None)
        {
            fadeState = enFadeState.None;
            NextState();
        }
    }

    IEnumerator None()
    {
        while(fadeState==enFadeState.None)
        {
            fadeState = enFadeState.FadeOut;
            yield return null;
        }
        NextState(); // 이거안넣음
    }

    IEnumerator FadeOut()
    {
        float alpha = 0f;

        while (fadeState == enFadeState.FadeOut)
        {
            if(alpha < 1f)
            {
                alpha += Time.deltaTime / fadeOutDelay;
            }
            else
            {
                fadeState = enFadeState.ChangeBackg;
            }

            alpha = Mathf.Clamp(alpha, 0, 1);
            backImage.color = new Color(backImage.color.r, backImage.color.g, backImage.color.b, alpha); // r을 a로씀
            yield return null;
        }

        NextState();
    }

    IEnumerator ChangeBackg()
    {
        yield return null;
        Debug.Log("리소스의로딩, UI처리등등");

        fadeState = enFadeState.FadeIn;
        NextState();
    }

    IEnumerator FadeIn()
    {
        float alpha = 1f;

        while (fadeState == enFadeState.FadeIn)
        {
            if (alpha > 0f)
            {
                alpha -= Time.deltaTime / fadeInDelay; // - 안붙임
            }
            else
            {
                fadeState = enFadeState.Done;
            }

            alpha = Mathf.Clamp(alpha, 0, 1);
            backImage.color = new Color(backImage.color.r, backImage.color.g, backImage.color.b, alpha);
            yield return null;
        }

        NextState();
    }

    IEnumerator Done()
    {
        yield return null;

        fadeState = enFadeState.None;
    }

    private void NextState()
    {
        MethodInfo mInfo = this.GetType().GetMethod(fadeState.ToString(), BindingFlags.Instance | BindingFlags.NonPublic);
        iStateCo = (IEnumerator)mInfo.Invoke(this, null);
        StartCoroutine(iStateCo);
    }
}
// 

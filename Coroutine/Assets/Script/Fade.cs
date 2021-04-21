// 미완성

using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public enum eFadeState
{
    None,
    FadeOut,
    ChangeBG,
    FadeIn,
    Done
}

public class Fade : MonoBehaviour
{
    eFadeState fadeState;
    Image imgBg;
    IEnumerator iStateCo = null;

    private void Awake()
    {
        imgBg = this.gameObject.GetComponent<Image>();
        if(imgBg == null)
        {
            Debug.Log("이미지 컴포넌트 없음");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && fadeState == eFadeState.None)
        {
            NextState();
        }
    }

    IEnumerator None() // 결과 반환
    {
        yield return null;
        fadeState = eFadeState.FadeOut;
        NextState();
    }

    IEnumerator FadeOut()
    {
        float alpha = 0f;
        while(fadeState == eFadeState.FadeOut)
        {
            if(alpha < 1)
            {
                alpha += Time.deltaTime;
            }
            else
            {
                fadeState = eFadeState.ChangeBG;
            }

            alpha = Mathf.Clamp(alpha, 0, 1);
            Color c = imgBg.color;
            c.a = alpha;
            imgBg.color = c;
            // imgBg.color = new color: 좋은 건 아님.
            yield return new WaitForSeconds(0.5f);
        }
        NextState();
    }



    private void Die()
    {
        Debug.Log("asdasd"); // NextState에서불러옴
    }
    
    void NextState()
    {
        MethodInfo mInfo = this.GetType().GetMethod(fadeState.ToString(), BindingFlags.NonPublic | BindingFlags.Instance);
        iStateCo = (IEnumerator)mInfo.Invoke(this, null);
        StartCoroutine(iStateCo);
        //Player a = new Player();
        //a.name = "나선환";
        //Player b = new Player();
        //b.name = "그분";

        //MethodInfo m = Type.GetType("Player").GetMethod("Move", BindingFlags.Public | BindingFlags.Instance);

        //m.Invoke(this, null); // a넣으면 나선환나오고 b넣으면 그분이오십니다
    }
}

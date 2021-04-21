using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

    void NextState()
    {
        Method mthis.GetType().GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance);
    }
}

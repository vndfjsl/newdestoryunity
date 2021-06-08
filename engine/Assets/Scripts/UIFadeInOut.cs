using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeInOut : MonoBehaviour
{
    public Image thisImage;
    public Text thisText;
    public int fadeInOut = 1; // 밝아지냐고

    // Update is called once per frame
    void Update()
    {
        if (thisImage != null)
        {
            if (thisImage.color.a >= 0.95f) // 충분히 밝아졌으면 다시 어두워지게
            {
                fadeInOut = -1; // -1은 어두워짐
            }
            if (thisImage.color.a <= 0.05f)
            {
                fadeInOut = 1;
            }
            thisImage.color = new Color(thisImage.color.r, thisImage.color.g, thisImage.color.b, thisImage.color.a + (Time.deltaTime * 1.5f * fadeInOut));
        }

        if (thisText != null)
        {
            if (thisText.color.a >= 0.95f) // 충분히 밝아졌으면 다시 어두워지게
            {
                fadeInOut = -1; // -1은 어두워짐
            }
            if (thisText.color.a <= 0.05f)
            {
                fadeInOut = 1;
            }
            thisText.color = new Color(thisText.color.r, thisText.color.g, thisText.color.b, thisText.color.a + (Time.deltaTime * 1.5f * fadeInOut));
        }
    }
}

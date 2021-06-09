using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject tutorialUI = null;
    public GameObject menuUI = null;

    
    public void OnTutorial()
    {
        tutorialUI.SetActive(true);
    }
    public void OffTutorial()
    {
        tutorialUI.SetActive(false);
    }

    public void OnMenu()
    {
        menuUI.SetActive(true);
    }
    public void OffMenu()
    {
        menuUI.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}

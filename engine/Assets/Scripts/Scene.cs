using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void Enter1Stage()
    {
        SceneManager.LoadScene("Game");
    }

    public void Enter2Stage()
    {
        // TODO..
    }

    public void TurnBackToStage()
    {
        GameManager.Instance.stage++;
        GameManager.Instance.SkillUnlock();
        GameManager.Instance.drawPanel.SetActive(false);
        GameManager.Instance.playerWinPanel.SetActive(false);
        GameManager.Instance.enemyWinPanel.SetActive(false);
    }
}

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
        MoveMap.Instance.stage++;
        MoveMap.Instance.SkillUnlock();
        MoveMap.Instance.drawPanel.SetActive(false);
        MoveMap.Instance.playerWinPanel.SetActive(false);
        MoveMap.Instance.enemyWinPanel.SetActive(false);
    }
}

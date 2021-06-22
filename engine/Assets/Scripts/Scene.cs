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
}

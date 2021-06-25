using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMovement : MonoBehaviour
{
    public int index;
    public void MoveScene() { SceneManager.LoadScene(index); }

    private void Start()
    {
        Image image = GetComponent<Button>().image;
        image.alphaHitTestMinimumThreshold = 0.1f; // alpha가 0.1f 미만이면 안눌리게
    }


}

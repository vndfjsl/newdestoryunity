using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coroutine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FiveSecond());
    }
    IEnumerator FiveSecond()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Debug.Log("5��");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorButton : MonoBehaviour
{
    public int buttonIndex = 0;

    public void PressButton()
    {
        MoveMap.Instance.PressKey(buttonIndex);
    }
}

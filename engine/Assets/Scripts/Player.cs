using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int currentX = 0;
    public int currentY = 1;

    public List<int> nextBehavior = new List<int>();
    public int behaviorIndex = 0;

    public void Move()
    {
        switch (nextBehavior[behaviorIndex])
        {
            case (int)Behavior.UP: // À§
                if (currentY > 0)
                {
                    currentY -= 1;
                }
                break;

            case (int)Behavior.DOWN: // ¾Æ·¡
                if (currentY < 2)
                {
                    currentY += 1;
                }
                break;

            case (int)Behavior.LEFT: // ¿Þ
                if (currentX > 0)
                {
                    currentX -= 1;
                }
                break;

            case (int)Behavior.RIGHT: // ¿À
                if (currentX < 3)
                {
                    currentX += 1;
                }
                break;
        }
    }

    public void InitBehavior()
    {
        nextBehavior.Clear();
    }
}

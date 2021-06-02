using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentX = 3;
    public int currentY = 1;

    public int[] nextBehavior = {(int)Behavior.UP, (int)Behavior.LEFT, (int)Behavior.DOWN, (int)Behavior.DOWN, (int)Behavior.LEFT, (int)Behavior.RIGHT };
    public int behaviorIndex = 0;

    public void Move()
    {
        switch(nextBehavior[behaviorIndex])
        {
            case (int)Behavior.UP: // ��
                if (currentY > 0)
                {
                    currentY -= 1;
                }
                break;
            case (int)Behavior.DOWN: // �Ʒ�
                if (currentY < 2)
                {
                    currentY += 1;
                }
                break;
            case (int)Behavior.LEFT: // ��
                if (currentX > 0)
                {
                    currentX -= 1;
                }
                break;
            case (int)Behavior.RIGHT: // ��
                if (currentX < 3)
                {
                    currentX += 1;
                }
                break;
        }
        behaviorIndex++;
    }
}

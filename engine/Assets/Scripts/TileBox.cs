using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBox : MonoBehaviour
{
    // Start is called before the first frame update
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}

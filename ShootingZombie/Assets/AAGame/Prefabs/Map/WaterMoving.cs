using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMoving : MonoBehaviour
{
    internal float scrollSpeedX = 0.5f;
    internal float scrollSpeedY = 0;

    private Renderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        Vector2 offset = new Vector2(offsetX, offsetY);
        spriteRenderer.material.SetTextureOffset("_MainTex", offset);
    }
}

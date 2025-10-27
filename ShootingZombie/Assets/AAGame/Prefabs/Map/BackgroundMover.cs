using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMover : MonoBehaviour
{
    public bool NearTransformer = false;
    public bool MidTransformer = false;
    public bool FarTransformer = false;
    internal float scrollSpeedX = 0.5f;
    internal float scrollSpeedY = 0;

    float offsetX; 
    private Renderer spriteRenderer;
    [SerializeField]
    private BackGroundItem backGroundItem;

    void Start()
    {
        backGroundItem = GetComponentInParent<BackGroundItem>();
        spriteRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (NearTransformer)
        {
            offsetX += backGroundItem.MoveNear;

            Vector2 offset = new Vector2(offsetX, 0);
            spriteRenderer.material.SetTextureOffset("_MainTex", offset);
        }
        if (MidTransformer)
        {
            offsetX += backGroundItem.MoveMid;

            Vector2 offset = new Vector2(offsetX, 0);
            spriteRenderer.material.SetTextureOffset("_MainTex", offset);
        }
        if (FarTransformer)
        {
            offsetX += backGroundItem.MoveFar;

            Vector2 offset = new Vector2(offsetX, 0);
            spriteRenderer.material.SetTextureOffset("_MainTex", offset);
        }
    }
}

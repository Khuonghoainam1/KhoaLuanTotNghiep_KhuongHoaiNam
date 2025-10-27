using UnityEngine;

public class ParallaxLayerMap : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] sprites;

    private void OnEnable()
    {
        GameEvent.OnSetupMap.RemoveListener(SetUpMap);
        GameEvent.OnSetupMap.AddListener(SetUpMap);
    }

    private void SetUpMap()
    {
        spriteRenderer.sprite = sprites[GameManager.Instance.mapNumber];
    }
}

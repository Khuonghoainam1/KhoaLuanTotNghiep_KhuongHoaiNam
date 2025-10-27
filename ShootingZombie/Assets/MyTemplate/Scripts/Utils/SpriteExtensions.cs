using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpriteExtensions
{
    public static Texture2D ToTexture2D(this Sprite sprite)
    {
        if (sprite != null)
        {
            if (sprite.rect.width != sprite.texture.width)
            {
                Texture2D newText = new Texture2D((int)sprite.textureRect.width, (int)sprite.textureRect.height);
                Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                             (int)sprite.textureRect.y,
                                                             (int)sprite.textureRect.width,
                                                             (int)sprite.textureRect.height);
                newText.SetPixels(newColors);
                newText.Apply();
                return newText;
            }
            else
                return sprite.texture;
        } else {
            Debug.LogError("Thiếu ảnh kiếm tra lại item");
            return new Texture2D(100, 100, TextureFormat.Alpha8, true);
        }
    }
}

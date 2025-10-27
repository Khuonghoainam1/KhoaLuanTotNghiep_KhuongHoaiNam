using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Thanh.Core;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteManager", menuName = "Toan/Sprite Manager")]
public class SpriteManager : ScriptableObject
{
    private static SpriteManager instance;
    public static SpriteManager Instance
    {
        get
        {
            if (instance == null)
                instance = Resources.Load<SpriteManager>("Manager/SpriteManager");
            return instance;
        }
    }

    [SerializeField]
    List<ItemSprite> itemSprites;

    public Sprite GetSprite(ItemID itemID, int index = 0)
    {
        ItemSprite itemSprite = itemSprites.Find(x => x.ID == itemID);
        if (itemSprite != null)
        {
            if (index == 0)
            {
                return itemSprite.sprite;
            }
            else
            {
                return itemSprite.others[index - 1];
            }
        }
        return null;
    }

    public Sprite GetSpriteTheme(ItemID itemID)
    {
        ItemSprite itemSprite = itemSprites.Find(x => x.ID == itemID);
        if(itemSprite != null)
        {
            if(BaseScene.theme == Theme.Normal)
            {
                return itemSprite.sprite;
            }
            else
            {
                if (itemSprite.others.Count > 0)
                {
                    return itemSprite.others[0];
                }
            }
        }
        return null;
    }

    public Sprite GetSpriteOther(ItemID id, int index = 0)
    {
        ItemSprite itemSprite = itemSprites.Find(x => x.ID == id);
        if(itemSprite != null)
        {
            if(itemSprite.others.Count > 0)
            {
                return itemSprite.others[0];
            }
        }

        return null;
    }
}

[Serializable]
public class ItemSprite
{
    public ItemID ID;
    public Sprite sprite;
    public List<Sprite> others;

    public ItemSprite(ItemID iD, Sprite sprite)
    {
        ID = iD;
        this.sprite = sprite;
    }
}
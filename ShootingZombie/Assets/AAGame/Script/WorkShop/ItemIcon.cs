using AA_Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : Item
{
    public Image icon;

    public void SetData(ItemID ID)
    {
        icon.sprite = SpriteManager.Instance.GetSprite(ID);
    }
}

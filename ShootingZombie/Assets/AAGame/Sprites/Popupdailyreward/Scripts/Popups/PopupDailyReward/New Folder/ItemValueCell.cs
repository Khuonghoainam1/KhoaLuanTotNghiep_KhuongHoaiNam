using System.Collections;
using System.Collections.Generic;
/*using System.Security.Policy;*/
using Thanh.Core;
using TMPro;
using UnityEngine;

public class ItemValueCell : ItemCell
{
    public TextMeshProUGUI txtValue;
    public int value;
    public bool isSkinHero;


    public virtual void SetDataForItem(ItemValue itemValue, string x = null, bool skipIcon = false, bool toKMB = true)
    {
       SetData(itemValue.item, itemValue.item, skipIcon);
        value = itemValue.value;
        icon.sprite = SpriteManager.Instance.GetSprite(itemID);
        //icon.SetNativeSize();
        if (itemValue.item == ItemID.thorAmount)
        {
            icon.sprite = SpriteManager.Instance.GetSprite(itemID,1);
            icon.SetNativeSize();
        }


        if (isSkinHero)
        {
            icon.sprite = SpriteManager.Instance.GetSprite(ItemID.SkinFree);
        }
        txtValue.text = (x != null ? x : "x") + (toKMB ? itemValue.value.ToKMB() : itemValue.value.ToString());
    }
    public virtual void SetData()
    {
        icon.sprite = SpriteManager.Instance.GetSprite(itemID);
    }
}

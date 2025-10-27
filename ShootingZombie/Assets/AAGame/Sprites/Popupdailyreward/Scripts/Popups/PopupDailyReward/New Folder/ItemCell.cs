using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Thanh.Core
{
    public class ItemCell : MonoBehaviour
    {
        public Image icon;

        [HideInInspector]
        public ItemID itemID = ItemID.None;

        public virtual void SetData(ItemID _itemID, ItemID nameOther, bool skipIcon = false)
        {
            itemID = _itemID;

            if (skipIcon == false)
            {
                if (nameOther != ItemID.None)
                {
                    icon.sprite = SpriteManager.Instance.GetSprite(ItemID.Gold);
                    //icon.SetNativeSize();
                }
                else
                {
                    icon.sprite = SpriteManager.Instance.GetSprite(itemID);
                }
                //icon.SetNativeSize();
            }
        }

        public virtual void Reset()
        {
            itemID = ItemID.None;

            icon.sprite = null;
        }
    }
}
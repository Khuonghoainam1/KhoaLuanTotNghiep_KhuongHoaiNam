using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpinCell : MonoBehaviour
{
    public ItemID id;
    public TextMeshProUGUI txt;
    public Image icon;

    public void SetData(ItemID _id, int count)
    {
        id = _id;

        if (id == ItemID.Gold  )
        {
            icon.sprite = SpriteManager.Instance.GetSprite(id);
           // icon.SetNativeSize();
            txt.text = count.ToString();
        }
        else if( id == ItemID.thorAmount)
        {
            icon.sprite = SpriteManager.Instance.GetSprite(id,1);
            txt.text = count.ToString();
           // icon.SetNativeSize();
        }
        else if(id == ItemID.SkinFree)
        {
            icon.sprite = SpriteManager.Instance.GetSprite(id);
            txt.text = count.ToString();
        }
        else
        {
            icon.sprite = SpriteManager.Instance.GetSprite(_id);
            icon.SetNativeSize();
            if (ItemType.IsWeapon(_id))
            {
                txt.text = "WEAPON";
                txt.fontSize = 24;
            }
            else if (ItemType.IsSkin(_id))
            {
                txt.text = "SKIN";
                txt.fontSize = 36;
            }
            else
            {
                txt.text = "Thor Amout";
                txt.fontSize = 36;
            }

            icon.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
    }
}
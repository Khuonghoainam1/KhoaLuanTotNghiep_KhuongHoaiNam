using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellReview : MonoBehaviour
{
    public ItemID ItemID;
    public Image Icon;
    public void OnEnable()
    {
        SetUp();
    }
    public void SetUp()
    {
        Icon.sprite  = SpriteManager.Instance.GetSprite(ItemID);
       Icon.SetNativeSize();
    }
}

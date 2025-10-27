using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconShopInven : MonoBehaviour
{
    public Image itemIcon;
    public TMP_Text txtPrice;
    public TMP_Text IconCollect;
    public ItemValue itemvalue;
    public ShopInventory inventory;
    public GameObject btnFree;
    [SerializeField] private Sprite btnShopYellow;
    [SerializeField] private Sprite btnShopGreen;
    [SerializeField] private Sprite btnShopBlue;
    [SerializeField] private Image bg;
    [SerializeField] private GameObject FxIcon;
    public void SetUp(ShopInventory _shopinventory, TypeShop type)
    {
        this.inventory = _shopinventory;
        IconCollect.text = inventory.itemcell.value.ToString();
        itemIcon.sprite = _shopinventory.spriteIcon;
        txtPrice.text = inventory.txtPr.ToString() + "$";
        if(inventory.txtPr == 0)
        {
            btnFree.SetActive(true);
            txtPrice.gameObject.SetActive(false);
            bg.sprite = btnShopYellow;
            FxIcon.SetActive(false);
        }
        else if(inventory.txtPr != 0)
        {
            btnFree.SetActive(false);
            txtPrice.gameObject.SetActive(true);
            FxIcon.SetActive(true);
            //bg.sprite = btnShopGreen;
            if (type == TypeShop.ShopGold)
            {
                bg.sprite = btnShopBlue;
            }
            else if (type == TypeShop.ShopHammer)
           {
                bg.sprite = btnShopGreen;
           }
        }
    }   
    public void OnClaim()
    {
        int amountIcon = inventory.itemcell.value;
        if(inventory.itemcell.item == ItemID.Gold)
        {


          
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
                (pop) =>
                {
                    pop.SetData(inventory.itemcell);
                    PopupClaimItem.Instane.btnX2.gameObject.SetActive(false);
                    PopupClaimItem.Instane.OkCollect.gameObject.SetActive(true);
                });
            User.Instance[ItemID.Gold] += amountIcon;
        }
        else if (inventory.itemcell.item == ItemID.thorAmount) {

           
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
                (pop) =>
                {
                    pop.SetData(inventory.itemcell);
                    PopupClaimItem.Instane.btnX2.gameObject.SetActive(false);
                    PopupClaimItem.Instane.OkCollect.gameObject.SetActive(true);
                });
            User.Instance[ItemID.thorAmount] += amountIcon;
        }
    }
}

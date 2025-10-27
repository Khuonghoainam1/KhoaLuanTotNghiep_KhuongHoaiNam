using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : Popup
{

    [SerializeField] private Button btnLstGold;
    [SerializeField] private Button btnLstHammer;


    public List<ShopInventory> _ShopGold = new List<ShopInventory>();
    public List<ShopInventory> _ShopHammer =  new List<ShopInventory>();
    [SerializeField] private Transform gunGroup;
    [SerializeField] private TypeShop TypeInventory;
    [SerializeField] private GameObject prefabCell;
    [SerializeField] private Sprite btnOn;
    [SerializeField] private Sprite btnOff;

    private void OnEnable()
    {
        TypeInventory = TypeShop.ShopGold;
       
        btnLstGold.onClick.RemoveListener(ClickbtnGold);
        btnLstGold.onClick.AddListener(ClickbtnGold);

        btnLstHammer.onClick.RemoveListener(ClickbtnHammer);
        btnLstHammer.onClick.AddListener(ClickbtnHammer);
        Setup();
    }
    private void Setup()
    {
        foreach (Transform trans in gunGroup)
        {
            Destroy(trans.gameObject);
        }
        if (TypeInventory == TypeShop.ShopGold)
        {
            btnLstGold.image.sprite = btnOn;
            btnLstHammer.image.sprite = btnOff;
            btnLstGold.image.SetNativeSize();
            btnLstHammer.image.SetNativeSize();
            foreach(ShopInventory shopGold in _ShopGold)
            {
                IconShopInven iconshopivent = Instantiate(prefabCell , gunGroup).GetComponent<IconShopInven>();
                iconshopivent.transform.parent = gunGroup;
                iconshopivent.SetUp(shopGold, TypeInventory);
            }
           
        }
        else if (TypeInventory == TypeShop.ShopHammer)
        {
            btnLstHammer.image.sprite = btnOn;
            btnLstGold.image.sprite = btnOff;
            btnLstHammer.image.SetNativeSize();
            btnLstGold.image.SetNativeSize();
            foreach (ShopInventory shopGold in _ShopHammer)
            {
                IconShopInven iconshopivent = Instantiate(prefabCell, gunGroup).GetComponent<IconShopInven>();
                iconshopivent.transform.parent = gunGroup;
                iconshopivent.SetUp(shopGold, TypeInventory);
            }
        }
       
    }

    void ClickbtnGold()
    {
        TypeInventory = TypeShop.ShopGold;
        AudioManager.instance.Play("BtnClick");
        Setup();
    }
    void ClickbtnHammer()
    {
        TypeInventory = TypeShop.ShopHammer;
        AudioManager.instance.Play("BtnClick");
        Setup();
    }
}
public enum TypeShop
{
    ShopGold,
    ShopHammer,
}

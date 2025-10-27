using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;

public class PopupCollection : Popup
{
    #region
    /*[SerializeField] private List<ItemValue> lstReWard = new List<ItemValue>();
    [SerializeField] private List<CellCollection> lstCellCollect = new List<CellCollection>();
    private void OnEnable()
    {
        for (int i = 0; i < lstCellCollect.Count; i++)
        {
            lstCellCollect[i].Setdata(lstReWard[i].item, lstReWard[i].value);
        }
    }
    void OnClaim(int index)
    {
        var intemRewaed = lstReWard[index];
        int gold = intemRewaed.value;
        if(intemRewaed.item == ItemID.Gold)
        {
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
                (pop) => { pop.SetData(intemRewaed);
                });
            User.Instance[ItemID.Gold] += gold;
        }
        if (gold > 0)
        {
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
            {
                ItemValue reward = new ItemValue(ItemID.Gold, gold);
                pop.SetData(reward, 1);
            });

        }
    }*/
    #endregion

    [SerializeField] private List<GunInventory> guns = new List<GunInventory>();
    [SerializeField] private List<GunInventory> ExtraGuns = new List<GunInventory>();
    [SerializeField] private Button btnClickSungChinh;
    [SerializeField] private Button btnClickSungPhu;
    [SerializeField] private Transform gunGroup;
    [SerializeField] private GunInventory gunInventory;
    [SerializeField] private TypeCollection typeCollection;
    [SerializeField] private GameObject prefabCellGuns;
    [SerializeField] private Sprite btnOn;
    [SerializeField] private Sprite btnOff;
    [SerializeField] private Button btnNext;
    
    private void OnEnable()
    {
        typeCollection = TypeCollection.SungChinh;
       

        btnClickSungChinh.onClick.RemoveAllListeners();
        btnClickSungChinh.onClick.AddListener(ClickSungChinh);

        btnClickSungPhu.onClick.RemoveAllListeners();
        btnClickSungPhu.onClick.AddListener(ClickSungPhu);
        SetUp();

    }
    public void SetUp()
    {
        gunInventory = null;
        foreach(Transform trans in gunGroup)
        {
            Destroy(trans.gameObject);
        }
        if(typeCollection == TypeCollection.SungChinh)
        {
            foreach(GunInventory gun in guns)
            {
                CellCollection cellgunButton = Instantiate(prefabCellGuns, gunGroup).GetComponent<CellCollection>();
                cellgunButton.transform.parent = gunGroup;
                cellgunButton.SetUp(gun);
                if(gunInventory == null && cellgunButton.inventoryGun.isUnlock == false)
                {
                    this.gunInventory = cellgunButton.inventoryGun;
                }
            }
            btnClickSungChinh.image.sprite = btnOn;
            btnClickSungPhu.image.sprite = btnOff;
        }
        else if(typeCollection == TypeCollection.SungPhu)
        {
            foreach (GunInventory gun in ExtraGuns)
            {
                CellCollection cellgunButton = Instantiate(prefabCellGuns, gunGroup).GetComponent<CellCollection>();
                cellgunButton.transform.parent = gunGroup;
                cellgunButton.SetUp(gun);
                if (gunInventory == null && cellgunButton.inventoryGun.isUnlock == false)
                {
                    this.gunInventory = cellgunButton.inventoryGun;
                }
            }
            btnClickSungChinh.image.sprite = btnOff;
            btnClickSungPhu.image.sprite = btnOn;
        }
    }
    public void ClickSungChinh()
    {
        typeCollection = TypeCollection.SungChinh;
        SetUp();

    }
    public void ClickSungPhu()
    {
        typeCollection = TypeCollection.SungPhu;
        SetUp();
    }
}
public enum TypeCollection
{
    SungChinh,
    SungPhu,
}

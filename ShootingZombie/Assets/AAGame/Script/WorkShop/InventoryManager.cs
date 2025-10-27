using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Popup
{
    public List<GunInventory> guns = new List<GunInventory>();
    public List<GunInventory> listExtraGun = new List<GunInventory>();
    public Transform gunGroup;
    public GameObject gunButtonPrefab;
    public GunInforUI gunInfor;
    GunInventory gunInventory;
    public TypeGunUpgrade typeGunUpgrade;
    public Button tabSungChinhBtn;
    public Button tabSungPhuBtn;

    private void OnEnable()
    {
        typeGunUpgrade = TypeGunUpgrade.SungChinh;
        GameEvent.OnUnLockGun.RemoveListener(SetUpNotSelect);
        GameEvent.OnUnLockGun.AddListener(SetUpNotSelect);


        tabSungChinhBtn.onClick.RemoveAllListeners();
        tabSungChinhBtn.onClick.AddListener(OnTabSungChinh);

        tabSungPhuBtn.onClick.RemoveAllListeners();
        tabSungPhuBtn.onClick.AddListener(OnTabSungPhu);
    
        SetUp();
        gunInfor.SetUp(this.gunInventory);
    }

    public void SetUp()
    {
        gunInventory = null;
     
        foreach (Transform trans in gunGroup)
        {
            Destroy(trans.gameObject);
        }

        if (typeGunUpgrade == TypeGunUpgrade.SungChinh)
        {
            foreach (GunInventory gun in guns)
            {
                GunButtonInventory gunButton = Instantiate(gunButtonPrefab, gunGroup).GetComponent<GunButtonInventory>();
                gunButton.transform.parent = gunGroup;
                gunButton.SetUp(gun);
                if (gunInventory == null && gunButton.gunInventory.isUnlock == false)
                {
                    this.gunInventory = gunButton.gunInventory;
                    gunButton.FirstSelect();
                }
            }
        }
        else
        {
            foreach (GunInventory gun in listExtraGun)
            {
                GunButtonInventory gunButton = Instantiate(gunButtonPrefab, gunGroup).GetComponent<GunButtonInventory>();
                gunButton.transform.parent = gunGroup;
                gunButton.SetUp(gun);
                if (gunInventory == null && gunButton.gunInventory.isUnlock == false)
                {
                    this.gunInventory = gunButton.gunInventory;
                    gunButton.FirstSelect();
                }
            }
        }
    }

    public void SetUpNotSelect()
    {
        gunInventory = null;
      
        foreach (Transform trans in gunGroup)
        {
            Destroy(trans.gameObject);
        }

        if (typeGunUpgrade == TypeGunUpgrade.SungChinh)
        {
            foreach (GunInventory gun in guns)
            {
                GunButtonInventory gunButton = Instantiate(gunButtonPrefab, gunGroup).GetComponent<GunButtonInventory>();
                gunButton.transform.parent = gunGroup;
                gunButton.SetUp(gun);
                if (gunButton.gunInventory.gunName == gunInfor.gunInventory.gunName)
                {
                    this.gunInventory = gunButton.gunInventory;
                    gunButton.FirstSelect();
                }
            }
        }
        else
        {
            foreach (GunInventory gun in listExtraGun)
            {
                GunButtonInventory gunButton = Instantiate(gunButtonPrefab, gunGroup).GetComponent<GunButtonInventory>();
                gunButton.transform.parent = gunGroup;
                gunButton.SetUp(gun);
                if (gunButton.gunInventory.gunName == gunInfor.gunInventory.gunName)
                {
                    this.gunInventory = gunButton.gunInventory;
                    gunButton.FirstSelect();
                }
            }
        }
    }


    public void OnTabSungChinh()
    {
        typeGunUpgrade = TypeGunUpgrade.SungChinh;
        SetUp();
        gunInfor.SetUp(this.gunInventory);
    }

    public void OnTabSungPhu()
    {
        typeGunUpgrade = TypeGunUpgrade.SungPhu;
        SetUp();
        gunInfor.SetUp(this.gunInventory);
    }
}

public enum TypeGunUpgrade
{
    SungChinh,
    SungPhu,
}


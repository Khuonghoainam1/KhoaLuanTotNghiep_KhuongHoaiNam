using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellCollection : MonoBehaviour
{

    [SerializeField] private TMP_Text txtCoin;
    [SerializeField] private Image icon;
    [SerializeField] private bool unLook = false;
    [SerializeField] private Button btnClame;
    [SerializeField] private Sprite bgUnlockFalse;//btn chua unlock
    [SerializeField] private Sprite bgUnlockTrue;//btn Chuan bi lock
    [SerializeField] private Sprite bgUnlocker;
    //btn da unlock setactive false;
    [SerializeField] private GameObject objTick;
    [SerializeField] private TMP_Text txtReward;
    [SerializeField] private Image imgcoin;
    public GunInventory inventoryGun;
    public SkeletonGraphic skeBtnClaim;
    public Image bgr;

    public bool StatusClaim = false;
    //public ItemValue itemcell;
    public void OnEnable()
    {
        btnClame.onClick.RemoveListener(OnClaim);
        btnClame.onClick.AddListener(OnClaim);

        Init();
    }

    public void Init()
    {
    }
    public void SetUp(GunInventory _gunInventory)
    {
        this.inventoryGun = _gunInventory;
        txtCoin.text = inventoryGun.itemcell.value.ToString();
        //  icon.color = Color.black;
        icon.sprite = Resources.Load<Sprite>("Sprite/Gun/" + inventoryGun.gunName.ToString());
        //  icon.SetNativeSize();\
       
        if (inventoryGun.isUnlock == false)
        {
            icon.color = Color.black;
            // btnClame.image.sprite = btnUnlockFalse;
            skeBtnClaim.AnimationState.SetAnimation(0, "0_blue", true);
            skeBtnClaim.Skeleton.SetSlotsToSetupPose();
            btnClame.interactable = false;
            bgr.sprite = bgUnlockFalse;
            //  objTick.gameObject.SetActive(false);
        }
        else if (inventoryGun.isUnlock == true)
        {
            icon.color = Color.white;
            btnClame.interactable = true;
            //btnClame.image.sprite = btnClaimUnlock;
            skeBtnClaim.AnimationState.SetAnimation(0, "0_yellow", true);
            //objTick.gameObject.SetActive(true);
            bgr.sprite = bgUnlockTrue;
        }

        if (User.Instance.lstGunclaim().Contains(this.inventoryGun.gunID))
        {
            btnClame.gameObject.SetActive(false);
            SetColerBtn();
            objTick.gameObject.SetActive(true);
        }
    }

    void OnClaim()
    {
        // var intemRewaed = lstReWard[index];
    
        int gold = inventoryGun.itemcell.value;
        if (inventoryGun.itemcell.item == ItemID.Gold)
        {
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem,
                (pop) =>
                {
                    pop.SetData(inventoryGun.itemcell);
                });
            User.Instance[ItemID.Gold] += gold;
            btnClame.gameObject.SetActive(false);
            User.Instance.lstGunclaim().Add(inventoryGun.gunID);
            objTick.gameObject.SetActive(true);
        }
        if (gold > 0)
        {
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
            {
                ItemValue reward = new ItemValue(ItemID.Gold, gold);
                pop.SetData(reward, 1);
            });

        }
        //bgr.sprite = bgUnlocker;
        SetColerBtn();
        Debug.Log("claimmm");
    }
    public void SetColerBtn()
    {
        bgr.color = new Color(1f, 1f, 1f, 0.5f);
        icon.color = new Color(1f, 1f, 1f, 0.5f);
        txtCoin.color = new Color(1f, 1f, 1f, 0.5f);
        txtReward.color = new Color(1f, 1f, 1f, 0.5f);
        imgcoin.color = new Color(1f, 1f, 1f, 0.5f);
    }
}

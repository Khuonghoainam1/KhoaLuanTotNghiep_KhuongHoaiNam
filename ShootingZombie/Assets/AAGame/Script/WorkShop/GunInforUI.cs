using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunInforUI : MonoBehaviour
{
    public TMP_Text nameGun;
    public TMP_Text title;
    public Image iconGun;
    public Button unlockButton;
    public TMP_Text unlocked;
    public Button equipBtn;
    public Button equipedBtn;

    public List<PieceGun> pieceGuns = new List<PieceGun>();
    public GunInventory gunInventory;
    public InventoryManager inventoryManager;
    public GameObject TutSelect;
    public GameObject[] tutTam;
    private void OnEnable()
    {
        GameEvent.OnViewGun.RemoveListener(SetUp);
        GameEvent.OnViewGun.AddListener(SetUp);
        //SetUp(inventoryManager.guns.GetRandom());
        unlockButton.onClick.RemoveAllListeners();
        unlockButton.onClick.AddListener(UnLockGun);

        GameEvent.OnUnLockGun.RemoveListener(UnLockGunDone);
        GameEvent.OnUnLockGun.AddListener(UnLockGunDone);

        equipBtn.onClick.RemoveListener(EquipGun);
        equipBtn.onClick.AddListener(EquipGun);



    }

    public void SetUp(GunInventory gunInventory)
    {
        this.gunInventory = gunInventory;
        //tutorial
        if (User.Instance[ItemID.TutSlectWorkShop] == 0)
        {
            TutSelect.gameObject.SetActive(true);
            for(int i= 0; i < tutTam.Length; i++)
            {
                tutTam[i].gameObject.SetActive(true);

            }
        }
        else
        {
            TutSelect.gameObject.SetActive(false);
            for (int i = 0; i < tutTam.Length; i++)
            {
                tutTam[i].gameObject.SetActive(false);

            }
        }

        nameGun.text = this.gunInventory.gunName.ToString();
        //title.text
        iconGun.sprite = Resources.Load<Sprite>("Sprite/Gun/" + this.gunInventory.gunName.ToString());
        SetAmountPieceGun();
        if (!CaculatePiece())
        {
            unlockButton.gameObject.SetActive(false);
        }
        else
        {
            unlockButton.gameObject.SetActive(true);
        }

        //neu sung da mo khoa roi thi se an nut unlock di
        if (this.gunInventory.isUnlock)
        {
            unlockButton.gameObject.SetActive(false);
            //unlocked.gameObject.SetActive(true);
        }
        else
        {
            //unlocked.gameObject.SetActive(false);
        }

        equipBtn.gameObject.SetActive(false);
        equipedBtn.gameObject.SetActive(false);
        if (this.gunInventory.isUnlock)
        {

            if (inventoryManager.typeGunUpgrade == TypeGunUpgrade.SungChinh)
            {
                if (this.gunInventory.gunID != User.Instance.GunUsing)
                {
                    equipBtn.gameObject.SetActive(true);
                    equipedBtn.gameObject.SetActive(false);
                }
                else
                {
                    equipBtn.gameObject.SetActive(false);
                    equipedBtn.gameObject.SetActive(true);
                   
                }
            }
            else
            {
                if (this.gunInventory.gunID != User.Instance.GunBotUsing)
                {
                    equipBtn.gameObject.SetActive(true);
                    equipedBtn.gameObject.SetActive(false);
                }
                else
                {
                    equipBtn.gameObject.SetActive(false);
                    equipedBtn.gameObject.SetActive(true);
                }
            }
        }
    }

    public void SetAmountPieceGun()
    {
        foreach (PieceGun pieceGun in pieceGuns)
        {
            pieceGun.gameObject.SetActive(false);
        }

        if (this.gunInventory.isUnlock)
        {
            return;
        }

        foreach (PieceNeed pieceNeed in gunInventory.pieceNeed)   //nhung manh sung can co
        {
            foreach (PieceGun pieceGun in pieceGuns)
            {
                if (pieceNeed.IDPiece == pieceGun.IDManhSung)
                {
                    pieceGun.gameObject.SetActive(true);
                    pieceGun.amountText.text = User.Instance[pieceNeed.IDPiece].ToString() +"/"+pieceNeed.amount.ToString();
                    pieceGun.SetUp(pieceNeed.amount);
                }
            }
        }
    }

    public bool CaculatePiece()
    {
        bool x = true;
        foreach (PieceNeed pieceNeed in gunInventory.pieceNeed)
        {
            if (pieceNeed.amount > User.Instance[pieceNeed.IDPiece])
            {
                x = false;
                break;
            }
        }
        return x;
    }

    public void UnLockGun()
    {
        foreach (PieceNeed pieceNeed in gunInventory.pieceNeed)
        {
            User.Instance[pieceNeed.IDPiece] -= pieceNeed.amount;
        }
        this.gunInventory.isUnlock = true;

        if (inventoryManager.typeGunUpgrade == TypeGunUpgrade.SungChinh)
        {
            User.Instance.ListUserGun().Add(this.gunInventory.gunID);
        }
        else
        {
            User.Instance.ListBotGun().Add(this.gunInventory.gunID);
        }
        
        User.Instance.Save();
        //refesh
        GameEvent.OnUnLockGun.Invoke();
    }

    public void UnLockGunDone()
    {
        SetUp(this.gunInventory);
    }

    /// <summary>
    /// User trang bi sung moi
    /// </summary>
    public void EquipGun()
    {
        if (inventoryManager.typeGunUpgrade == TypeGunUpgrade.SungChinh)
        {
            User.Instance.GunUsing = this.gunInventory.gunID;
        }
        else
        {
            User.Instance.GunBotUsing = this.gunInventory.gunID;
        }
        equipBtn.gameObject.SetActive(false);
        equipedBtn.gameObject.SetActive(true);

        //tutorial
        User.Instance[ItemID.TutSlectWorkShop] = 1;
        if (User.Instance[ItemID.TutSlectWorkShop]== 1)
        {
            for (int i = 0; i < tutTam.Length; i++)
            {
                tutTam[i].gameObject.SetActive(false);

            }
            TutSelect.SetActive(false);
            User.Instance[ItemID.TutSlectWorkShop] = 1;

        }


        GameEvent.OnEquiepGun.Invoke(null);
    }
}

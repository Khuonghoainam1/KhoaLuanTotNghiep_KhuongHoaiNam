using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AA_Game;

public class GunButtonInventory : Item
{
    public Button button;
    public Image icon;
    public TMP_Text nameGun;
    public TMP_Text desc;
    public TMP_Text status;
    public GameObject focus;

    public GunInventory gunInventory;

    private void OnEnable()
    {
        GameEvent.OnViewGun.RemoveListener(OnViewGun);
        GameEvent.OnViewGun.AddListener(OnViewGun);
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(ButtonSelect);
    }

    public void SetUp(GunInventory gunInventory)
    {
        this.gunInventory = gunInventory;
        icon.sprite = Resources.Load<Sprite>("Sprite/Gun/"+gunInventory.gunName.ToString());
        nameGun.text = this.gunInventory.gunName.ToString();

        //tinh toan hien  thi co the nang cap hay khong
        if (CaculatePiece())
        {
            desc.text = "Co the nang cap";
        }
        else
        {
            desc.text = "Chua du vat lieu";
        }


        //status UNLOCK
        if (User.Instance.ListUserGun().Contains(this.gunInventory.gunID) || User.Instance.ListBotGun().Contains(this.gunInventory.gunID))
        {
            this.gunInventory.isUnlock = true;
            status.text = "Da so huu";
            desc.gameObject.SetActive(false);
        }
        else
        {
            this.gunInventory.isUnlock = false;
            status.text = "Chua so huu";
            desc.gameObject.SetActive(true);
        }
    }

    public bool CaculatePiece()
    {
        bool x = true;
        foreach(PieceNeed pieceNeed in gunInventory.pieceNeed)
        {
            if (pieceNeed.amount > User.Instance[pieceNeed.IDPiece])
            {
                x = false;
                break;
            }
        }
        return x;
    }

    public void OnViewGun(GunInventory gunInventory)
    {
        focus.SetActive(false);
    }

    public void ButtonSelect()
    {
        GameEvent.OnViewGun.Invoke(gunInventory);
        focus.SetActive(true);
    }

    public void FirstSelect()
    {
        focus.SetActive(true);
    }
}

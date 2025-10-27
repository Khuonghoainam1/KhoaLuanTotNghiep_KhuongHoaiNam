using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunButton : MonoBehaviour
{
    public Button button;
    public Image BG;   //quuyet ding sung chinh hay sung phu
    public GameObject focusImage;  // bat len khi co sung cung level co the merge
    public Image iconGun;  // quyet dinh boi loai sung + level
    public GameObject blackImg; // bat len khi sung khong the merge
    public GameObject star; // cap cao se co sao
    public TMP_Text level;  // bang cap cua sung
    public GameObject watchAds; // sung unlock bang ads
    public GameObject goldLock; // sung unlock bang gold


    public Gun gun;
    public Gun gunBase;

    public void SetUp(Gun gun)
    {
        this.gun = gun;
        gunBase = this.gun;

        if (gun.isUnlock)
        {
            watchAds.SetActive(false);
            goldLock.SetActive(false);
            focusImage.SetActive(false);
            blackImg.SetActive(false);

            if (gun.isHaveGun)
            {
                iconGun.gameObject.SetActive(true);
                level.gameObject.SetActive(true);
                level.text = "LV. " + gun.level.ToString();
                star.gameObject.SetActive(true);

                iconGun.sprite = Resources.Load<Sprite>("Sprite/Gun/" + gun.type + gun.level);

                if (gun.type == TypeGun.SungChinh)
                {
                    BG.sprite = Resources.Load<Sprite>("Sprite/Gun/BGSungChinh");
                }
                else
                {
                    BG.sprite = Resources.Load<Sprite>("Sprite/Gun/BGSungPhu");
                }
            }
            else
            {
                iconGun.gameObject.SetActive(false);
                level.gameObject.SetActive(false);
                star.gameObject.SetActive(false);
            }
        }
        else
        {
            if (gun.typeUnlock == TypeUnLock.ByAds)
            {
                watchAds.SetActive(true);
                goldLock.SetActive(false);
            }
            else
            {
                watchAds.SetActive(false);
                goldLock.SetActive(true);
            }
        }
    }

    public void OnPointEnter()
    {
        Debug.Log("OnPointEnter-buttonTemp---" + gun.type);
        if (gun.isUnlock == false)
        {
            return;
        }

        GameScene.main.inventoryPanel.buttonTemp = this.gun;
        if (GameScene.main.inventoryPanel.buttonSellect != null)
        {
            this.gun = GameScene.main.inventoryPanel.buttonSellect;
        }
    }

    public void OnBeginDrag()
    {
        Debug.Log("gun---" + gun.type);
        if (this.gun.isUnlock == false || this.gun.isHaveGun == false)
        {
            return;
        }
        GameScene.main.inventoryPanel.buttonSellect = this.gun;
        Debug.Log("OnBeginDrag-buttonSellect--" + GameScene.main.inventoryPanel.buttonSellect.type);
    }

    public void OnEndDrag()
    {
        Debug.Log("OnEndDrag-gun---" + gun.type);
        if (this.gun.isHaveGun)
        {
            this.gun = GameScene.main.inventoryPanel.buttonTemp;
            GameScene.main.inventoryPanel.buttonTemp = null;
            GameScene.main.inventoryPanel.SaveInventory();
            GameScene.main.inventoryPanel.OnShow();
        }
    }

    public void OnPointExit()
    {
        Debug.Log("OnPointExit-gun---" + gun.type);
        GameScene.main.inventoryPanel.buttonTemp = null;
        this.gun = gunBase;
    }
}

[System.Serializable]
public class Gun
{
    public bool isUnlock;
    public TypeUnLock typeUnlock;
    public bool isHaveGun;
    public TypeGun type;
    public int level;
}

public enum TypeGun
{
    None,
    SungChinh,
    SungPhu,
}

public enum TypeUnLock
{
    ByAds,
    ByGold,
}

using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupClaimItem : Popup
{
    public Image imgIcon;
    public Button OkCollect;
    public Button btnX2;
    [SerializeField]
    private TextMeshProUGUI textVale;
    private int value;
    private ItemValue itemValue;
    [SerializeField]
    private Image halo;
  
    [SerializeField]
    private Button btnNothanks;
    public static PopupClaimItem Instane;
    public void Awake()
    {
        Instane = this;
    }

    public bool setSkin;
    private int Value
    {
        get { return value; }
        set
        {
            this.value = value;
            if (itemValue.item == ItemID.thorAmount)
            {
                imgIcon.sprite = SpriteManager.Instance.GetSprite(itemValue.item, 1);
                imgIcon.SetNativeSize();
                textVale.text = "+" + this.Value.ToString();
                textVale.gameObject.SetActive(true);

                return;
            }
            textVale.gameObject.SetActive(true);

            textVale.text = "+" + this.Value.ToString();
            if(itemValue.item == ItemID.Gold)
            {
                imgIcon.sprite = SpriteManager.Instance.GetSprite(itemValue.item);
                imgIcon.SetNativeSize();
            }
            imgIcon.sprite = SpriteManager.Instance.GetSprite(itemValue.item);
        }
    }
    public override void OnShow()
    {
        base.OnShow();
        AudioManager.instance.Play("unlockitem");
    
    }
    public void OnEnable()
    {
        halo.transform.DORotate(new Vector3(0, 0, 360), 2f, DG.Tweening.RotateMode.FastBeyond360).OnComplete(() =>
        {
            //Close();
            btnNothanks.onClick.RemoveAllListeners();
            btnNothanks.onClick.AddListener(() => Close());
        });
        StartCoroutine(InstbtnX2());

      /*  OkCollect.onClick.RemoveListener(Close);
        OkCollect.onClick.AddListener(Close);
*/

      
    }
    public void SetData(ItemValue _itemValue, int XValue = 1)
    {
        //AudioAssistant.PlaySound("UnlockNew");
        itemValue = _itemValue;
        Debug.Log(itemValue.item);
        Value = itemValue.value;
      
      

          OkCollect.onClick.RemoveListener(Close);
        OkCollect.onClick.AddListener(Close);
        btnX2.onClick.RemoveAllListeners();
        btnX2.onClick.AddListener(() =>
        {
            ClickBtnX2();
            Close();
        });


    }
  
    public override void Close()
    {
        base.Close();
        btnNothanks.gameObject.SetActive(false);
    }
    public void ClickBtnX2()
    {
        int gold = itemValue.value;
        if (gold > 0)
        {
            PopupManager.Instance.OpenPopup<PopupClaimItem>(PopupID.PopupClaimItem, (pop) =>
            {

                ItemValue reward = new ItemValue(itemValue.item, gold);

                pop.SetData(reward, 1);

            });

        }
        if (itemValue.item == ItemID.thorAmount)
        {

            User.Instance[ItemID.thorAmount] += gold;
        }
        else if (itemValue.item == ItemID.Gold)
        {
            User.Instance[ItemID.Gold] += gold;
        }
        Debug.Log(itemValue.item);

    }

 
    IEnumerator InstbtnX2()
    {
        yield return new WaitForSeconds(3f);
        btnNothanks.gameObject.SetActive(true);
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Thanh.Core;
using Spine.Unity;
using System.Collections;

public class InforPopup : Popup
{
    public TMP_Text nameTalent;
   // public Image imageBlock;
    public Image icon;
    public RectTransform bg;
    public TMP_Text desc;
    public Button unlockBtn;
    public Image iconPrice;
    public TMP_Text price;

    private Talent talent;
    public Sprite[] priceSprites;
    [SerializeField] private GameObject imgLight;
    public SkeletonGraphic btnUnlockGold;
    public TMP_Text txtWarning;
    public GameObject tutInforTalent;

    public void OnEnable()
    {
        unlockBtn.onClick.RemoveListener(Unlock);
        unlockBtn.onClick.AddListener(Unlock);
    }
    public override void Close()
    {
        base.Close();
        txtWarning.gameObject.SetActive(false);
    }
    public void SetUp(Talent talent)
    {
        this.talent = talent;
        nameTalent.text = talent.talentType.ToString();

        if (User.Instance[ItemID.Gold] >= talent.gold)
        {
            btnUnlockGold.AnimationState.SetAnimation(0, "0_yellow", true);
            btnUnlockGold.Skeleton.SetSlotsToSetupPose();
        }
        else if (User.Instance[ItemID.Gold] < talent.gold)
        {
            btnUnlockGold.AnimationState.SetAnimation(0, "0_green", true);
            btnUnlockGold.Skeleton.SetSlotsToSetupPose();
        }
      


        if (talent.talentType == TalentType.Damage || talent.talentType == TalentType.HP || talent.talentType == TalentType.Healing)
        {
            if ((talent.indexTalent == User.Instance[ItemID.talentCurrent]) && talent.level <= User.Instance[ItemID.PlayingLevel])
            {
                unlockBtn.gameObject.SetActive(true);
                iconPrice.sprite = priceSprites[0];
            }
            else
            {
                unlockBtn.gameObject.SetActive(false);
            }

            bg.anchoredPosition = new Vector3(0, -80, 0);
        }
        else  //set unlock for special talent
        {
            if ((talent.indexTalent == User.Instance[ItemID.talentCurrentSpecial]) && talent.level * 3 < User.Instance[ItemID.talentCurrent])
            {
                unlockBtn.gameObject.SetActive(true);
                iconPrice.sprite = priceSprites[1];
            }
            else
            {
                unlockBtn.gameObject.SetActive(false);
            }

            bg.anchoredPosition = new Vector3(0, -20, 0);
        }


        icon.sprite = Resources.Load<Sprite>("Sprites/Talent/" + talent.talentType.ToString());
        icon.SetNativeSize();

        price.text = talent.gold.ToKMB();

        //desc
        if (talent.talentType == TalentType.Damage)
        {
            desc.text = "Damage +" + talent.amountUp.ToString().Replace(talent.amountUp.ToString(), "<color=#00FFF5>" + talent.amountUp.ToString());
        }
        else if (talent.talentType == TalentType.HP)
        {
            desc.text = "HP +" + talent.amountUp.ToString().Replace(talent.amountUp.ToString(), "<color=#00FFF5>" + talent.amountUp.ToString());
        }
        else if (talent.talentType == TalentType.Healing)
        {
            desc.text = "Healing +" + talent.amountUp.ToString().Replace(talent.amountUp.ToString(), "<color=#00FFF5>" + talent.amountUp.ToString());
        }
        else
        {
            desc.fontSize = 40f;
            desc.text = talent.des;
        }
      
      
        //tut
        if(this.talent.level ==1 && User.Instance[ItemID.tutTalent] < 1)
        {
            tutInforTalent.SetActive(true);
            btnClose.interactable = false;
        }
        else
        {
            btnClose.interactable = true;
            tutInforTalent.SetActive(false);
        }
    }

    public void Unlock()
    {
        if (User.Instance[ItemID.Gold] >= talent.gold)
        {
            btnUnlockGold.AnimationState.SetAnimation(0, "0_yellow", true);
            btnUnlockGold.Skeleton.SetSlotsToSetupPose();
        }
        else if (User.Instance[ItemID.Gold] < talent.gold)
        {
            btnUnlockGold.AnimationState.SetAnimation(0, "0_green", true);
            btnUnlockGold.Skeleton.SetSlotsToSetupPose();
        }
        if (User.Instance[ItemID.ticket] >= talent.gold)
        {
            btnUnlockGold.AnimationState.SetAnimation(0, "0_yellow", true);
            btnUnlockGold.Skeleton.SetSlotsToSetupPose();
        }
        else if (User.Instance[ItemID.ticket] < talent.gold)
        {
            btnUnlockGold.AnimationState.SetAnimation(0, "0_green", true);
            btnUnlockGold.Skeleton.SetSlotsToSetupPose();
        }
      
        if (talent.talentType == TalentType.Damage || talent.talentType == TalentType.HP || talent.talentType == TalentType.Healing)
        {
            if (User.Instance[ItemID.Gold] >= talent.gold)
            {
                //done tut
                if (User.Instance[ItemID.tutTalent] < 1)
                {
                    User.Instance[ItemID.tutTalent] = 1;
                    GameScene.main.homePanel.tutTalent.SetActive(false);
                }

                User.Instance[ItemID.Gold] -= talent.gold;
                User.Instance[ItemID.talentCurrent] += 1;
                User.Instance.UserTalents().Add(this.talent);
                SetUp(this.talent);
                GameEvent.OnUnlockTalent.Invoke();
                txtWarning.gameObject.SetActive(false);
                this.Close();
            }
            else
            {
                //Debug.Log("KHONG CO DU VANG");
                if (User.Instance[ItemID.tutTalent] < 1)
                {
                    User.Instance[ItemID.tutTalent] = 1;
                    GameScene.main.homePanel.tutTalent.SetActive(false);

                    User.Instance[ItemID.Gold] = 0;
                    User.Instance[ItemID.talentCurrent] += 1;
                    User.Instance.UserTalents().Add(this.talent);
                    SetUp(this.talent);
                    GameEvent.OnUnlockTalent.Invoke();
                    txtWarning.gameObject.SetActive(false);
                    this.Close();
                }

                txtWarning.gameObject.SetActive(true);
            }
        }
        else
        {
            if (User.Instance[ItemID.ticket] >= talent.gold)
            {
                User.Instance[ItemID.ticket] -= talent.gold;
                User.Instance[ItemID.talentCurrentSpecial] += 1;
                User.Instance.UserTalents().Add(this.talent);
                SetUp(this.talent);
                GameEvent.OnUnlockTalent.Invoke();
                txtWarning.gameObject.SetActive(false);
                this.Close();
            }
            else
            {
                txtWarning.gameObject.SetActive(true);

                //Debug.Log("KHONG CO DU TIKET");
            }
        }
        AudioManager.instance.Play("levelUp");
    }
    IEnumerator ShowText()
    {
       
        
        yield return new WaitForSeconds(1f);
        txtWarning.gameObject.SetActive(false);

    }
}

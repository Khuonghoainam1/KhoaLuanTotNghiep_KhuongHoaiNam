using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTalent : MonoBehaviour
{
    public Button selectButton;
    public Image lockImage;
    public Image lineConnect;
    public Image icon;
    public Talent talent;
    private bool isUnlock;
    public Sprite imgYellow;
    public Sprite imgGreen;
    public Sprite imgOrage;
    public Image imgLine;
    public bool isSpecialTalent;
    public TMP_Text txtDes;

    [SerializeField] private Image imgButton;
    [SerializeField] private Sprite sprButtonGray;
    [SerializeField] private Sprite sprButtonBlue;
    [SerializeField] private Sprite sprButtonGold;
    [SerializeField] private GameObject imgTick;
    public LevelTalentUI parent;
    public GameObject tutTalent;


    private void OnEnable()
    {
        selectButton.onClick.RemoveListener(OpenInforTalent);
        selectButton.onClick.AddListener(OpenInforTalent);
        GameEvent.OnUnlockTalent.RemoveListener(OnUnlockTalent);
        GameEvent.OnUnlockTalent.AddListener(OnUnlockTalent);
    }
        
    public void SetUp(Talent talent,int level)
    {
        this.talent = talent;
        this.talent.level = level;
        icon.sprite = Resources.Load<Sprite>("Sprites/Talent/" + talent.talentType.ToString());
        icon.SetNativeSize();

        txtDes.text = "+" + talent.amountUp.ToString();
        //kiem tra xem talent nay da duoc mo hay chua
        foreach (Talent talentUser in User.Instance.UserTalents())
        {
            if(talentUser.level == this.talent.level && talentUser.talentType == this.talent.talentType)
            {
                //da mo khoa
                
                isUnlock = true;

                if (imgLine != null)
                {
                    imgLine.sprite = imgOrage;
                }
                imgTick.gameObject.SetActive(true);
                imgButton.sprite = sprButtonGray;
                lockImage.gameObject.SetActive(true);
                break;
            }
            else
            {
                //chua mo khoa
                
                isUnlock = false;

                lockImage.gameObject.SetActive(false) ;

                if (imgLine != null)
                {
                    imgLine.sprite = imgGreen;
                }
                imgTick.gameObject.SetActive(false);
                imgButton.sprite = sprButtonBlue;


                if (isSpecialTalent)
                {
                    if (talent.indexTalent == User.Instance[ItemID.talentCurrentSpecial])
                    {
                        imgTick.gameObject.SetActive(false);
                        lockImage.gameObject.SetActive(false);
                        if ((talent.indexTalent == User.Instance[ItemID.talentCurrentSpecial]) && talent.level * 3 < User.Instance[ItemID.talentCurrent])
                        {
                            imgButton.sprite = sprButtonGold;
                        }
                    }
                }
                else
                {
                    if (talent.indexTalent == User.Instance[ItemID.talentCurrent])
                    {
                        imgLine.sprite = imgYellow;
                        imgTick.gameObject.SetActive(false);
                        imgButton.sprite = sprButtonGold;
                        lockImage.gameObject.SetActive(false);

                        //
                        StartCoroutine(ShowBg());
                        
                    }
                }
            }
        }


        if(this.talent.level == 1 && User.Instance[ItemID.tutTalent] < 1 && tutTalent != null)
        {
            tutTalent.SetActive(true);
        }
        else if(tutTalent != null)
        {
            tutTalent.SetActive(false);
        }
    }

    IEnumerator ShowBg()
    {
        yield return new WaitForSeconds(0.2F);
        parent.bg.SetActive(true);
    }

    public void OpenInforTalent()
    {
        if (tutTalent != null && User.Instance[ItemID.tutTalent] < 1)
        {
            tutTalent.SetActive(false);
        }
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<InforPopup>(PopupID.PopupInforTalent, (pop) => pop.SetUp(this.talent));
    }

    public void OnUnlockTalent()
    {
        if (!isSpecialTalent)
        {
            parent.bg.SetActive(false);
        }
        AudioManager.instance.Play("BtnClick");
        SetUp(this.talent, this.talent.level);
    }
}

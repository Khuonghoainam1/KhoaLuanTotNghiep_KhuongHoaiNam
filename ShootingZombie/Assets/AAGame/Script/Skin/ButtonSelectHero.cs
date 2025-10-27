using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectHero : MonoBehaviour
{
    public GameObject imageSelect;
    public Button selectBtn;
    public HeroSelect heroSelect;
    public Image iconHero;
    public SkinData currentSkin;

    private void OnEnable()
    {
        GameEvent.OnSelectSkinOld.RemoveListener(SetIconHero);
        GameEvent.OnSelectSkinOld.AddListener(SetIconHero);



        SetUp();
        SetIconHero();
    }

    public void SetUp()
    {
        if (heroSelect == HeroSelect.Player)
        {
            imageSelect.SetActive(true);
        }
        else
        {
            imageSelect.SetActive(false);
        }


        selectBtn.onClick.RemoveAllListeners();
        selectBtn.onClick.AddListener(SelectHeroBtn);

        GameEvent.OnSelectHero.RemoveListener(OnSelectHero);
        GameEvent.OnSelectHero.AddListener(OnSelectHero);

    }


    public void SelectHeroBtn()
    {
        GameEvent.OnSelectHero.Invoke(this.heroSelect);
        imageSelect.SetActive(true);
    }

    public void OnSelectHero(HeroSelect heroSelect)
    {
        imageSelect.SetActive(false);
    }

    public void SetIconHero(string temp=null)
    {
        if (heroSelect == HeroSelect.Player)
        {
            GetCurrentSkin(User.Instance.CurrentPlayerSkin);
        }
        else if (heroSelect == HeroSelect.Bot1)
        {
            GetCurrentSkin(User.Instance.CurrentBot1Skin);
        }
        else if (heroSelect == HeroSelect.Bot2)
        {
            GetCurrentSkin(User.Instance.CurrentBot2Skin);
        }
        else if (heroSelect == HeroSelect.Bot3)
        {
            GetCurrentSkin(User.Instance.CurrentBot3Skin);
        }

        iconHero.sprite = currentSkin.skinIcon;
    }

    public void GetCurrentSkin(ItemID skin)
    {
        foreach (SkinData sk in SkinManager.Instance.skinDatas)
        {
            if (sk.skinID == skin)
            {
                currentSkin = sk;
            }
        }
    }
}

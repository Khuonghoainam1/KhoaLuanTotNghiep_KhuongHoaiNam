using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.GameCore;

public class HeroesPopup : Popup
{
    public SkeletonGraphic skeletonGraphic;
    public SkeletonDataAsset dataAsset;

    public List<string> skins;
    public List<string> skinsNV;


    public Transform groupSkin;
    public Button equipBtn;

    public HeroSelect heroSelect;
    public static SkinData currentSkin;

    public SkinData skinTemp;

    private void Awake()
    {
        skinTemp = (SkinData)ScriptableObject.CreateInstance("SkinData");
    }


    public void OnEnable()
    {
        heroSelect = HeroSelect.Player;
        GameEvent.OnSelectHero.RemoveListener(OnSelectHero);
        GameEvent.OnSelectHero.AddListener(OnSelectHero);

        GameEvent.OnSelectSkinOld.RemoveListener(SetSkin);
        GameEvent.OnSelectSkinOld.AddListener(SetSkin);



        //clear
        foreach (Transform trans in groupSkin)
        {
            if (trans != null)
            {
                Destroy(trans.gameObject);
            }
        }


        foreach (ItemID skin in User.Instance.ListCharSkin())
        {
            //SkinData skinData = new SkinData();
            foreach (SkinData sk in SkinManager.Instance.skinDatas)
            {
                if (sk.skinID == skin)
                {
                    skinTemp = sk;
                }
            }

            ItemSkin itemSkin = ContentPoolable.Emit(ItemID.itemSkin) as ItemSkin;
            itemSkin.transform.parent = groupSkin.transform;
            itemSkin.transform.localScale = Vector3.one;
            //itemSkin.SetUp(skinTemp);
        }


        GameEvent.OnSelectHero.Invoke(HeroSelect.Player);
    }


    public void SetSkin(string skinEquip)
    {
        skins.Clear();
        if (skins.Count < dataAsset.GetSkeletonData(true).Skins.Items.Where(x => x != null).Count())
        {
            foreach (var skin in dataAsset.GetSkeletonData(true).Skins.Items.Where(x => x != null))
            {
                skins.Add(skin.ToString());
            }
        }

        skinsNV.Clear();
        foreach (string skin in skins.Where(x => x.StartsWith("Char")))
        {
            skinsNV.Add(skin);
        }

        foreach (string skin in skinsNV)
        {
            if (skin.Contains(skinEquip))
            {
                skeletonGraphic.Skeleton.SetSkin(skin);
                skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                break;
            }
        }
    }


    public void OnSelectHero(HeroSelect heroSelect)
    {
        this.heroSelect = heroSelect;

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

        SetSkin(currentSkin.skinID.ToString());
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

public enum HeroSelect
{
    Player,
    Bot1,
    Bot2,
    Bot3,
}

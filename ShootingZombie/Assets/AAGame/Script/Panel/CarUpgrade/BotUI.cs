using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class BotUI : MonoBehaviour
{
    public UserBot userBot;
    public int botIndex;
    public SkeletonGraphic skeletonGraphic;
    public SkeletonDataAsset[] skeletonDataAsset; // PLAYER----->-BOT1----->-BOT2----->-BOT3
    public bool isIntoTheCar;

    private void OnEnable()
    {
        if (isIntoTheCar)
        {
            GameEvent.OnEquipSkin.RemoveListener(OnEquipSkin);
            GameEvent.OnEquipSkin.AddListener(OnEquipSkin);

            GameEvent.OnSelectSkin.RemoveListener(OnSelectSkin);
            GameEvent.OnSelectSkin.AddListener(OnSelectSkin);
        }
    }

    public void SetUp(UserBot userBot,int botIndex)
    {
        this.userBot = userBot;
        this.botIndex = botIndex;
        skeletonGraphic.Skeleton.SetSkin(userBot.skin.ToString());
        //skeletonGraphic.Skeleton.SetSlotsToSetupPose();

       // ChangeDataAsset(userBot);
    }

    public void OnSelectSkin(UserBot userBot)
    {
        if(this.userBot.type == userBot.type)
        {
            skeletonGraphic.Skeleton.SetSkin(userBot.skin.ToString());
            //skeletonGraphic.Skeleton.SetSlotsToSetupPose();
        }
    }

    public void OnEquipSkin(UserBot userBot)
    {
        switch (botIndex)
        {
            case 0:
                this.userBot = User.Instance.UserPlayerUsing;
                skeletonGraphic.Skeleton.SetSkin(this.userBot.skin.ToString());
                //skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                break;
            case 1:
                this.userBot = User.Instance.UserBot1Using;
                skeletonGraphic.Skeleton.SetSkin(this.userBot.skin.ToString());
                //skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                break;
            case 2:
                this.userBot = User.Instance.UserBot2Using;
                skeletonGraphic.Skeleton.SetSkin(this.userBot.skin.ToString());
                //skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                break;
            case 3:
                this.userBot = User.Instance.UserBot3Using;
                skeletonGraphic.Skeleton.SetSkin(this.userBot.skin.ToString());
               // skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                break;
        }

    }

    public void SetSkin(UserBot userBot)
    {
        ChangeDataAsset(userBot);
    }

    public void ChangeDataAsset(UserBot userBot)
    {
        if (userBot.type == TypeBot.Player)
        {
            skeletonGraphic.skeletonDataAsset = skeletonDataAsset[0];
        }
        if (userBot.type == TypeBot.Pistol)
        {
            skeletonGraphic.skeletonDataAsset = skeletonDataAsset[1];
        }
        if (userBot.type == TypeBot.Riffle)
        {
            skeletonGraphic.skeletonDataAsset = skeletonDataAsset[2];
        }
        if (userBot.type == TypeBot.Bazoka)
        {
            skeletonGraphic.skeletonDataAsset = skeletonDataAsset[3];
        }

        skeletonGraphic.Initialize(true);
        skeletonGraphic.Skeleton.SetSkin(userBot.skin.ToString());
    }
}

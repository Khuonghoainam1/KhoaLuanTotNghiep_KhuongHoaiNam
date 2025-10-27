using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Yurowm.GameCore;

public static class SpineGraphicUtils
{
    public static SkeletonGraphic Spawn(ItemID heroID, int skinIdx, AnimID animID, int animIdx, Transform parent)
    {
        CharacterSkin characterData = ContentPoolable.GetPrefab<CharacterSkin>(x => x.id == heroID);
        SpineData spineData = characterData.spineData;
        SkeletonDataAsset dataAsset = spineData.dataAsset;
        Material material = spineData.skeletonMaterial;

        dataAsset.GetSkeletonData(false);
        var sg = SkeletonGraphic.NewSkeletonGraphicGameObject(dataAsset, parent, material); // Spawn a new SkeletonGraphic GameObject.
        sg.gameObject.name = "SkeletonGraphic Instance";

        //Extra Stuff
        sg.Initialize(false);
        sg.Skeleton.SetSkin(spineData.skins[Mathf.Clamp(skinIdx, 0, spineData.skins.Count - 1)]);
        sg.Skeleton.SetSlotsToSetupPose();
        AnimInfo animInfo = characterData.GetAnimInfo(animID);
        sg.AnimationState.SetAnimation(0, animInfo.animDatas.GetRandom().anim, true);
        sg.raycastTarget = false;
        sg.unscaledTime = true;

        return sg;
    }
}

public static class SpineUtils
{
    public static List<string> GetBoneNames(this SkeletonDataAsset dataAsset)
    {
        SkeletonData skeletonData = dataAsset.GetSkeletonData(false);
        return skeletonData.Bones.Select(x => x.Name).ToList();
    }
}
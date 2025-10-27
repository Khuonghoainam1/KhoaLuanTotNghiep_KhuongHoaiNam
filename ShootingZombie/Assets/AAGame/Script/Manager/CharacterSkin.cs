using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using Spine.Unity;
using Spine.Unity.Examples;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Yurowm.GameCore;
using AA_Game;
using Spine;

public class CharacterSkin : Item
{
    public SkeletonAnimation anim;
    public Spine.Skeleton skeleton;
    public MeshRenderer render;

    public string characterName;
    public string description;
    public FloatReference moveSpeed;
    public float visionRadius;
    public float safeZone;
    public Transform boneTay;
    public SkeletonGhost ghost;
    public Skin customSkin;
    [TabGroup("Animation")]
    [TableList(AlwaysExpanded = true, DrawScrollView = true, MaxScrollViewHeight = 4000, MinScrollViewHeight = 2000)]
    public SpineData spineData;

    [TabGroup("Animation")]
    [TableList(AlwaysExpanded = true, DrawScrollView = true, MaxScrollViewHeight = 2000, MinScrollViewHeight = 1000)]
    public List<AnimInfo> animInfos;

    private void OnValidate()
    {
        if (anim == null)
        {
            anim = GetComponentInChildren<SkeletonAnimation>();
        }

        anim.loop = false;
        //render = anim.GetComponent<MeshRenderer>();
        //render.SetSortingLayer(SortingLayerID.Character, 12);

        if (characterName == "")
        {
            characterName = id.ToString();
        }

        spineData.skeAnim = anim;
        spineData.Validate(this);

        foreach (AnimInfo animInfo in animInfos)
        {
            animInfo.Validate(this);

            foreach (AnimData animData in animInfo.animDatas)
            {
                animData.Validate(this);
                animData.id = animInfo.id;
                //animData.effects.ForEach(x => x.bone == spineData.dataAsset.GetBoneNames(););
            }
        }

        spineData.dataAsset = anim.skeletonDataAsset;
        spineData.skeletonMaterial = spineData.dataAsset.atlasAssets[0].Materials.ElementAt(0);
    }

    public void SetSkin(string skin)
    {
        skeleton = anim.Skeleton;
        customSkin = new Skin("Hello");
        Skin charSkin = anim.Skeleton.Data.FindSkin(skin);
        customSkin.AddSkin(charSkin);

        skeleton.SetSkin(customSkin);
        skeleton.SetSlotsToSetupPose();
    }
    public AnimInfo GetAnimInfo(AnimID id)
    {
        AnimInfo info = animInfos.FirstOrDefault(x => x.id == id);
        if (info == null)
        {
            Debug.LogError("Miss anim " + id.ToString() + " in " + id.ToString());
        }
        return info;
    }

    public BoneData GetBoneData(BoneID boneID)
    {
        return spineData.GetBoneData(boneID);
    }




    public void CarSetSkin(string nameCar,string phase)
    {
        skeleton = anim.Skeleton;
        customSkin = new Skin("Hello");
        var skinStr = nameCar + "/" + phase;    /*spineData.skins.Find(x => x.Contains(nameCar));*/ 
        Skin charSkin = anim.Skeleton.Data.FindSkin(skinStr);
        customSkin.AddSkin(charSkin);
        skeleton.SetSkin(customSkin);
        skeleton.SetSlotsToSetupPose();
    }
}

#region Anim
[Serializable]
public class AnimInfo : SkinDataModule
{
    [TableColumnWidth(100, resizable: false)]
    public AnimID id;

    public List<AnimData> animDatas;
}

[Serializable]
public class SpineData : SkinDataModule
{
    public SkeletonDataAsset dataAsset;
    public Material skeletonMaterial;
    public SkeletonAnimation skeAnim;

    [SpineSkin(dataField: "dataAsset")]
    public List<string> skins;

    [SpineSkin(dataField: "dataAsset")]
    public List<string> skinsNV;

    [SpineSkin(dataField: "dataAsset")]
    public List<string> skinsVK;

    public List<string> AllBones;

    [TableList]
    public List<BoneData> boneDatas;

    public override void Validate(CharacterSkin _root)
    {
        base.Validate(_root);
        AllBones = dataAsset.GetBoneNames();
        boneDatas.ForEach(x => x.Validate(_root));
        skins.Clear();

        if (skins.Count < dataAsset.GetSkeletonData(true).Skins.Items.Where(x => x != null).Count())
        {
            foreach (var skin in dataAsset.GetSkeletonData(true).Skins.Items.Where(x => x != null))
            {
                skins.Add(skin.ToString());
            }
        }

        if (skins.Count > 1)
        {
            skinsNV.Clear();
            foreach (string skin in skins.Where(x => x.StartsWith("Char")))
            {
                skinsNV.Add(skin);
            }

            skinsVK.Clear();
            foreach (string skin in skins.Where(x => x.StartsWith("Shot") || x.StartsWith("Rock") || x.StartsWith("Rif") || x.StartsWith("Man")))
            {
                skinsVK.Add(skin);
            }
        }
    }

    public BoneData GetBoneData(BoneID boneID)
    {
        return boneDatas.Find(x => x.id == boneID);
    }
}

[Serializable]
public class BoneData : SkinDataModule
{
    public BoneID id;

    [ValueDropdown("GetBoneNames", ExpandAllMenuItems = true)]
    public string name;
}

[Serializable]
public class AnimData : SkinDataModule
{
    [HideInInspector]
    public AnimID id;

    public AnimationReferenceAsset anim;

    [HideInInspector]
    public float animEventNormal; // Với time scale = 1f
    public float animEvent;

    [HideInInspector]
    public float duration;

    [TableList]
    public List<AnimEffect> effects;

    private float _timeScale = 1f;
    public float timeScale
    {
        get
        {
            return _timeScale;
        }
        set
        {
            _timeScale = value;
            duration = anim.Animation.Duration / timeScale;
            animEvent = animEventNormal / timeScale;
            if (effects != null)
            {
                effects.ForEach(x => x.delay = x.delayNormal / timeScale);
            }
        }
    }

    public override void Validate(CharacterSkin _root)
    {
        base.Validate(_root);

        animEventNormal = animEvent;
        effects.ForEach(x => x.Validate(_root));

    }
}

[Serializable]
public class AnimEffect : SkinDataModule
{
    [TableColumnWidth(80, resizable: false)]
    public bool followBone;

    [TableColumnWidth(100, resizable: false)]
    [ValueDropdown("GetBoneNames", ExpandAllMenuItems = true)]
    public string bone;

    [HideInInspector]
    public float delayNormal;

    [TableColumnWidth(80, resizable: false)]
    public float delay;

    [ContentSelector]
    public List<FxItem> fxItems;

    public List<AudioClip> sounds;

    public override void Validate(CharacterSkin root)
    {
        base.Validate(root);

        delayNormal = delay;
    }
}

public class SkinDataModule
{
    CharacterSkin _root;
    public CharacterSkin root
    {
        get
        {
            return _root;
        }
        set
        {
            _root = value;
        }
    }

    public virtual void Validate(CharacterSkin _root)
    {
        this.root = _root;
    }

    [Searchable]
    public List<string> GetBoneNames
    {
        get
        {
            return root != null ? root.spineData.AllBones : null;
        }
    }
}

public enum AnimID
{
    attack_1,
    attack_2,
    die,
    hit,
    idle,
    run,
    spawn,
    idle_run,
    idle_stop,
    start,
    stop,
    shoot,
    victory,
    fixing,
    waving,
    jump,

    None = 1000
}

public enum BoneID
{
    TayTrai,
    TayPhai,
    ChanTrai,
    ChanPhai,
    Bung
}
#endregion
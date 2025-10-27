using Spine;
using Spine.Unity;
using System.Collections;
using System.Linq;
using UnityEngine;
using Yurowm.GameCore;

public class CharacterAnim : MonoBehaviour
{
    public CharacterSkin skin;
    public SkeletonAnimation anim;
    Spine.AnimationState animState;
    Spine.Skeleton skeleton;
    MeshRenderer render;
    public Event<string> OnHandleEvent = new Event<string>();



    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        anim = skin.anim;
        animState = anim.AnimationState;
        animState.Event -= HandleEvent;
        animState.Event += HandleEvent;
        skeleton = anim.Skeleton;
        curAnimID = AnimID.None;
    }


    void HandleEvent(TrackEntry trackEntry, Spine.Event e)
    {
        OnHandleEvent?.Invoke(e.Data.Name);
    }


    public AnimData PlayAnim(AnimID id, bool loop = false, float timeScale = 1f, bool playEffect = true)
    {
        AnimInfo info = GetAnimInfo(id);
        if (info == null)
        {
            info = GetAnimInfo(AnimID.idle);
        }
        AnimData animData = info.animDatas.GetRandom();
        return PlayAnim(animData, loop, timeScale, playEffect);
    }

    AnimID curAnimID = AnimID.None;

    public AnimID GetAnimID
    {
        get
        {
            return curAnimID;
        }
    }

    public AnimData PlayAnim(AnimData animData, bool loop = false, float timeScale = 1f, bool playEffect = true)
    {
        curAnimID = animData.id;

        // ClearTracks();

        AnimationReferenceAsset animRef = animData.anim;
        animData.timeScale = timeScale;
        TimeScale = timeScale;
        animState.SetAnimation(0, animRef, loop).TrackEnd = 2.14748365E+09f;

        if (animData.effects != null)
        {
            animData.effects.ForEach(x => PlayEffect(animData.id, playEffect, x));
        }

        return animData;
    }

    public float TimeScale
    {
        set
        {
            anim.timeScale = value;
            //root.data.moveSpeed = root.data.moveSpeedNormal * value;
        }

        get
        {
            return anim.timeScale;
        }
    }

    public void PlayEffect(AnimID id, bool playEffect, AnimEffect effect)
    {
        if (playEffect == false)
        {
            return;
        }

        StartCoroutine(IE_PlayEffect(id, effect));
    }

    IEnumerator IE_PlayEffect(AnimID id, AnimEffect effect)
    {
        if (effect.delay > 0)
        {
            yield return WaitForSecondsCache.Get(effect.delay);
        }

        if (effect.fxItems != null && effect.fxItems.Count > 0 && string.IsNullOrEmpty(effect.bone) == false)
        {
            Vector3 bonePos = GetBonePos(effect.bone);

            FxItem fxItem = ContentPoolable.Emit<FxItem>(effect.fxItems.GetRandom());
            BoneFollower boneFollower = fxItem.GetComponent<BoneFollower>();
            if (boneFollower != null)
            {
                Destroy(boneFollower);
            }
            if (effect.followBone)
            {
                fxItem.transform.parent = anim.transform;
                boneFollower = fxItem.gameObject.AddComponent<BoneFollower>();
                boneFollower.followBoneRotation = false;
                boneFollower.followXYPosition = true;
                boneFollower.followZPosition = false;
                boneFollower.followLocalScale = false;
                boneFollower.followSkeletonFlip = false;
                boneFollower.skeletonRenderer = anim;
                boneFollower.boneName = effect.bone;
                boneFollower.enabled = true;

            }
            else
            {
                fxItem.transform.parent = null;
            }

            fxItem.transform.position = bonePos;
            fxItem.transform.localScale = Vector3.one;
            fxItem.transform.rotation = Quaternion.identity;

        }

        //if ((BaseScene.currentSceneName != SceneName.GameScene || GameManager.Instance.State != GameState.Lose || GameManager.Instance.State != GameState.Win)
        //     && effect.sounds != null && effect.sounds.Count > 0)
        //{
        //    AudioAssistant.PlaySound(effect.sounds.GetRandom());
        //}
    }

    public void ClearTracks()
    {
        skeleton.SetToSetupPose();
        animState.ClearTracks();
    }

    public AnimInfo GetAnimInfo(AnimID id, float timeScale = 1f)
    {
        AnimInfo ret = skin.GetAnimInfo(id);
        if (ret != null)
        {
            ret.animDatas.ForEach(x => x.timeScale = timeScale);
        }
        return ret;
    }

    public AnimData GetAnimData(AnimID id, float timeScale = 1f)
    {
        AnimInfo animInfo = GetAnimInfo(id, timeScale);
        var animDatas = animInfo.animDatas;
        if (animDatas.Count() > 0)
        {
            return animDatas.GetRandom();
        }
        else
        {
            return animInfo.animDatas.GetRandom();
        }
    }

    public Vector3 GetBonePos(string boneName)
    {
        Bone bone = skeleton.FindBone(boneName);
        return anim.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0));
    }

    public void UpdateLayer(int layer)
    {
        render.SetSortingLayer(SortingLayerID.Character, layer);
    }
}

public enum Direction
{
    Left,
    Right
}
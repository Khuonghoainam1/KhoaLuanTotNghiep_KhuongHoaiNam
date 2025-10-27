using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class testskin : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;
    public SkeletonDataAsset dataAsset;

    public SkeletonDataAsset cu;


    private void Start()
    {
        skeletonGraphic.skeletonDataAsset = dataAsset;
        skeletonGraphic.Initialize(true);
        skeletonGraphic.Skeleton.SetSkin("char_deadpool");
        Invoke("DoiLai",2);
    }

    public void DoiLai()
    {
        skeletonGraphic.skeletonDataAsset = cu;
        skeletonGraphic.Initialize(true);
        skeletonGraphic.Skeleton.SetSkin("main");
    }
}

using Spine.Unity;
using UnityEngine;

public class GunSkinControl : MonoBehaviour
{
    public SkeletonGraphic skeletonGraphic;

    private void Start()
    {
        GameEvent.OnCarLevelUp.AddListener(SetSkinGun);
        SetSkinGun();
    }

    public void SetSkinGun()
    {
        skeletonGraphic.Skeleton.SetSkin(User.Instance.Car.gun.ToString());
    }
}

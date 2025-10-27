using Thanh.Core;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;

public class PopupNewUnlockSkin : Popup
{
    public SkeletonGraphic skeletonGraphic;
    public SkeletonDataAsset dataAsset;

    public List<string> skins;
    public List<string> skinsNV;

    public void SetData(string skinUnlock)
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

        foreach(string skin in skinsNV)
        {
            if (skin.Contains(skinUnlock))
            {
                skeletonGraphic.Skeleton.SetSkin(skin);
                skeletonGraphic.Skeleton.SetSlotsToSetupPose();
                break;
            }
        }
    }
}

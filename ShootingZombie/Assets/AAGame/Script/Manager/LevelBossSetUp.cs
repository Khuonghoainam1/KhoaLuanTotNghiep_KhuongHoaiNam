using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "LevelBoss", menuName = "LevelBossSetup/LevelBossData", order = 1)]
public class LevelBossSetUp : ScriptableObject
{
    public int level;
    public BossWorldName bossName;
    public Sprite avt;
    public ItemID bossId;
    public SkeletonDataAsset newSkeletonData;
    public string Name;
    public void ChangeSkeletonDataAsset(SkeletonGraphic skeletonGraphic )
    {
        // Assign the new SkeletonDataAsset
        skeletonGraphic.skeletonDataAsset = newSkeletonData;

        // Initialize the SkeletonGraphic with the new SkeletonDataAsset
        skeletonGraphic.Initialize(true);
    }
}


public enum BossWorldName
{
    Boss1,
    Boss2,
    Boss3,

}

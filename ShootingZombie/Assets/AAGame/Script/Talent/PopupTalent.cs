using Thanh.Core;
using UnityEngine;
using Yurowm.GameCore;

public class PopupTalent : Popup
{
    public TalentTableData talentTableData;
    public Transform group;
    public static int indexTalent = 1;
    public static int indexTalentSpecial = 1;

    public override void OnShow()
    {
        base.OnShow();
        indexTalent = 1;
        indexTalentSpecial = 1;
        foreach (LevelTalentData levelTalentData in talentTableData.levelTalentDatas)
        {
            LevelTalentUI levelTalentUI = ContentPoolable.Emit(ItemID.levelTalentUI) as LevelTalentUI;
            levelTalentUI.transform.parent = group;
            levelTalentUI.transform.localScale = Vector3.one;
            levelTalentUI.SetUp(levelTalentData);
        }
    }

    public override void Close()
    {
        base.Close();
        foreach(Transform trans in group)
        {
            Destroy(trans.gameObject);
        }
    }
}

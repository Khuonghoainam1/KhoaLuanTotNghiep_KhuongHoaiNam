using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AA_Game;

public class LevelTalentUI : Item
{
    public List<ItemTalent> itemTalents = new List<ItemTalent>();
    public TMP_Text levelTxt;


    public GameObject bg;
    private LevelTalentData levelTalentData;

    public void SetUp(LevelTalentData levelTalentData)
    {
        //set up cho mot level
        this.levelTalentData = levelTalentData;
        levelTxt.text = levelTalentData.level.ToString();



        //set up cho nhung talent trong 1 level
        for (int i = 0; i < levelTalentData.talents.Count; i++)
        {
            itemTalents[i].SetUp(levelTalentData.talents[i], levelTalentData.level);


            if (i < 3)  //chan special talent
            {
                itemTalents[i].talent.indexTalent = PopupTalent.indexTalent;
                PopupTalent.indexTalent += 1;
            }
            else
            {
                itemTalents[i].talent.indexTalent = PopupTalent.indexTalentSpecial;
                PopupTalent.indexTalentSpecial += 1;
            }
        }

        if (levelTalentData.talents.Count < 4)
        {
            itemTalents[3].gameObject.SetActive(false);
        }
    }
}

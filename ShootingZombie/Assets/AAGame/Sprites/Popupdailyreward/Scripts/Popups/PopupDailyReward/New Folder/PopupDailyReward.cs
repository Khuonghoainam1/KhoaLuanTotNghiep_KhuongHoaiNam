using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;

public class PopupDailyReward : Popup
{
    public List<DailyRewardCellView> dailyRewardCells;
    [SerializeField]
    private RectTransform timeCoutDown;
    [SerializeField]
    private RawImage imgRaw;
    public override void OnShow()
    {
        base.OnShow();
        SetUpDailyReWard();
        DailyRewardManager.Instance.OnRefeshDaily.RemoveAllListeners();
        DailyRewardManager.Instance.OnRefeshDaily.AddListener(SetUpDailyReWard);
        StartCoroutine(RunRawImage());
        this.gameObject.transform.localScale  = new Vector3(0.8f, 0.8f, 0.8f);
    }

    private void SetUpDailyReWard()
    {
        timeCoutDown.gameObject.SetActive(false);

        List<DailyRewardData> rewardDatas = DailyRewardManager.Instance.GetRewardDatas();
        for (int i = 0; i < dailyRewardCells.Count; i++)
        {
            if (i < rewardDatas.Count)
            {
                dailyRewardCells[i].SetData(rewardDatas[i]);
                dailyRewardCells[i].gameObject.SetActive(true);
            }
            else
            {
                dailyRewardCells[i].gameObject.SetActive(false);
            }
            if (rewardDatas[i].state == DailyRewardState.NotAchieve && rewardDatas[i - 1].state == DailyRewardState.Claimed)
            {
                AddTimeAndSetPos(dailyRewardCells[i].transform);
                timeCoutDown.gameObject.SetActive(true);
            }

        }
    }
    private void AddTimeAndSetPos(Transform posParent)
    {
        timeCoutDown.SetParent(posParent);
        timeCoutDown.anchoredPosition = new Vector2(6.9f, 16.33f);
    }
    IEnumerator RunRawImage()
    {
        float x = 0;
        float speed = 0.2f;
        while (true)
        {
            x += Time.deltaTime;
            Rect a = imgRaw.uvRect;
            a.x -= Time.deltaTime * speed;
            a.y -= Time.deltaTime * speed;
            imgRaw.uvRect = a;
            yield return null;
            continue;
        }
    }
}


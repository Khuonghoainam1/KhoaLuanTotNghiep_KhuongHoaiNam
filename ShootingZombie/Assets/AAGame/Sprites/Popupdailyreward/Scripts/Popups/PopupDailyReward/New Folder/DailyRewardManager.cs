using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DailyRewardManager : Singleton<DailyRewardManager>
{
    public DailyRewardPersistentData dailyRewardPersistentData;
    public DailyRewardPersistentData dailyRewardPersistentDataNew;
    //Path Save
    public const string DailyRewardPersistentDataPath = "DailyRewardPersistentDataPath";
    public const string DailyRewardPersistentDataPathNew = "DailyRewardPersistentDataPathOld";
    public static int LastDay = 7;
    public UnityEvent OnRefeshDaily = new UnityEvent();
    [TableList]
    public List<DailyRewardData> datas;// data load
    [SerializeField]
    ScheduleTask scheduleTask;


    public bool IsFinished()
    {
        return scheduleTask.IsFinished;
    }
    public string GetRemainTime()
    {
        return TimeSchedule.GetStringTime(scheduleTask.remainSeconds);
    }
    public int Day
    {
        get
        {
            return dailyRewardPersistentData.Day;
        }

        set
        {
            dailyRewardPersistentData.Day = value;
            Save();
        }
    }
    public int DayNew
    {
        get
        {
            return dailyRewardPersistentDataNew.Day;
        }

        set
        {
            dailyRewardPersistentDataNew.Day = value;
            Save();
        }
    }

    public void Clear()
    {
        DataPersistent.ClearData(DailyRewardPersistentDataPath);
        DataPersistent.ClearData(DailyRewardPersistentDataPathNew);
    }
    public List<DailyRewardData> GetRewardDatas()
    {
        List<DailyRewardData> ret = new List<DailyRewardData>();

        for (int i = 0; i < LastDay; i++)
        {
            ret.Add(datas[i]);
        }
        return ret;
    }
    public void Init()
    {
        //Load Data tu path
        scheduleTask = TimeSchedule.Instance.GetScheduleTask(ScheduleID.DailyReward);
        dailyRewardPersistentData = DataPersistent.ReadDataExist<DailyRewardPersistentData>(DailyRewardPersistentDataPath);
        dailyRewardPersistentDataNew = DataPersistent.ReadDataExist<DailyRewardPersistentData>(DailyRewardPersistentDataPathNew);
        if (dailyRewardPersistentData == null)
        {
            dailyRewardPersistentData = new DailyRewardPersistentData();

            Save();
        }
        if (dailyRewardPersistentDataNew == null)
        {
            dailyRewardPersistentDataNew = new DailyRewardPersistentData();
            Save();
        }

        // Lay thoi gian con lai ==0 la sang ngay moi
        if (TimeSchedule.Instance.GetScheduleTask(ScheduleID.DailyReward).remainSeconds == 0f)
        {
            dailyRewardPersistentData.Day = dailyRewardPersistentDataNew.Day;
        }
    }
    public void Save()
    {
        DataPersistent.SaveData<DailyRewardPersistentData>(DailyRewardPersistentDataPath, dailyRewardPersistentData);
        DataPersistent.SaveData<DailyRewardPersistentData>(DailyRewardPersistentDataPathNew, dailyRewardPersistentDataNew);
    }
    public void ClaimReward(bool isToDAy)
    {
        if (isToDAy)
        {
            Day++;
        }
        else
        {
            DayNew = Day + 1;
        }
        if(Day == 7)
        {
            Clear();
        }
        scheduleTask.Start();
        OnRefeshDaily?.Invoke();
        Debug.Log(dailyRewardPersistentDataNew.Day + "dailyRewardPersistentDataNew");
        Debug.Log(dailyRewardPersistentData.Day + "dailyRewardPersistentData");
    }
    public bool HasAchievedReward()
    {
        return GetRewardDatas().Find(x => x.state == DailyRewardState.Achieved) != null;
    }
    //Load Data For reward
    private void OnValidate()
    {
        datas = new List<DailyRewardData>();
        for (int i = 0; i < LastDay; i++)
        {
            DailyRewardData data = new DailyRewardData();
            data.day = i;
            data.itemValues = new List<ItemValue>();
            int dayInWeek = i % 7;
            ItemID skinFree;
            switch (i)
            {
                case 0:
                    data.itemValues.Add(new ItemValue(ItemID.Gold, 1000));
                    break;
                case 1:
                    data.itemValues.Add(new ItemValue(ItemID.thorAmount, 10));
                    break;
                case 2:
                    skinFree = ItemType.SkinsBot.GetRandom();
                    data.itemValues.Add(new ItemValue(skinFree, 1));
                    break;
                case 3:
                    data.itemValues.Add(new ItemValue(ItemID.Gold, 2500));
                    break;
                case 4:
                    data.itemValues.Add(new ItemValue(ItemID.thorAmount, 15));
                    break;
                case 5:
                    data.itemValues.Add(new ItemValue(ItemID.Gold, 10000));
                    break;
                case 6:

                    skinFree = ItemType.SkinsMain.GetRandom();
                    data.itemValues.Add(new ItemValue(skinFree, 1));
                    break;
            }
            datas.Add(data);
        }
    }
}
[System.Serializable]
public class DailyRewardData
{
    public int day;
    public List<ItemValue> itemValues;
    public DailyRewardState state
    {
        get
        {
            if (day < DailyRewardManager.Instance.Day || (day == DailyRewardManager.Instance.DayNew - 1 && DailyRewardManager.Instance.DayNew != 0))
            {
                //Ngay Hom Truoc
                return DailyRewardState.Claimed;
            }
            else if (day == DailyRewardManager.Instance.Day && DailyRewardManager.Instance.IsFinished())
            {
                //Ngay Hom Nay Nhung Chua Nhan
                return DailyRewardState.Achieved;
            }
            else if (day == DailyRewardManager.Instance.Day && !DailyRewardManager.Instance.IsFinished())
            {
                // Ngay Mai
                return DailyRewardState.CountDown;
            }
            else
            {
                // Ngay 
                return DailyRewardState.NotAchieve;
            }
        }
    }


}
public enum DailyRewardState
{
    NotAchieve,
    CountDown,
    Achieved,
    Claimed,
}
[System.Serializable]
public class DailyRewardPersistentData
{
    public int OpenPopupDay;
    public bool ClaimFirstReward;
    public int Day;

    public void Init()
    {
        OpenPopupDay = 0;
        ClaimFirstReward = false;
        Day = 0;
    }

    public DailyRewardPersistentData()
    {
        Init();
    }
}

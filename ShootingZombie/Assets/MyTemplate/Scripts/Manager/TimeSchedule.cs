using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Thanh.Core;

public class TimeSchedule : Singleton<TimeSchedule>
{
    [TableList]
    public List<ScheduleData> scheduleDatas;

    const string TimeSchedulePath = "TimeSchedulePath";
    public List<ScheduleTask> scheduleTasks;
    List<ScheduleTask> onlyOnlineTasks;

    public UnityEvent OnResetStartTime = new UnityEvent();

    public static bool cheatTime = false;

    public void Init()
    {
        scheduleDatas.ForEach(x => x.Init());
        Load();

       // AppFocusManager.AppPaused?.RemoveListener(OnAppPaused);
    //    AppFocusManager.AppPaused?.AddListener(OnAppPaused);
    }

    public void Load()
    {
        scheduleTasks = DataPersistent.ReadDataExist<List<ScheduleTask>>(TimeSchedulePath);
        if (scheduleTasks == null)
        {
            scheduleTasks = new List<ScheduleTask>();
        }

        foreach (ScheduleData scheduleData in scheduleDatas)
        {
            if (scheduleTasks.Find(x => x.id == scheduleData.id) != null)
            {
                continue;
            }

            ScheduleTask task = new ScheduleTask(scheduleData.id);
            scheduleTasks.Add(task);
        }

        scheduleTasks.RemoveAll(x => !scheduleDatas.Contains(y => y.id == x.id));

        onlyOnlineTasks = scheduleTasks.Where(x => GetScheduleData(x.id).onlyOnline).ToList();
        onlyOnlineTasks.ForEach(x =>
        {
            x.SetStartTime(TimeSchedule.GetTime().Value.AddSeconds(-x.onlineSeconds));
        });

        Save();
    }

    void OnAppPaused(bool isPaused)
    {
        if (cheatTime)
        {
            return;
        }

        if (isPaused)
        {
            scheduleTasks.ForEach(x =>
            {
                x.onlineSeconds = x.runSeconds;
            });
            Save();
        }
        else
        {
            scheduleTasks.ForEach(x =>
            {
                x.SetStartTime(TimeSchedule.GetTime().Value.AddSeconds(-x.onlineSeconds));
            });
            Save();
            OnResetStartTime?.Invoke();
        }
    }

    public void Clear()
    {
        DataPersistent.ClearData(TimeSchedulePath);
    }

    public void Save()
    {
        DataPersistent.SaveData<List<ScheduleTask>>(TimeSchedulePath, scheduleTasks);
    }

    public static DateTime? GetTime()
    {
        return DateTime.Now;
    }

    public ScheduleTask GetScheduleTask(ScheduleID scheduleID)
    {
        return scheduleTasks.Find(x => x.id == scheduleID);
    }

    public ScheduleData GetScheduleData(ScheduleID scheduleID)
    {
        return scheduleDatas.Find(x => x.id == scheduleID);
    }

    public static string GetStringTime(double remainSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(remainSeconds));
        if (timeSpan.Days == 0 && timeSpan.Hours == 0 && timeSpan.Minutes == 0)
        {
            return timeSpan.Seconds + "s";
        }
        else if (timeSpan.Days == 0 && timeSpan.Hours == 0)
        {
            return timeSpan.Minutes + "m " + timeSpan.Seconds + "s";
        }
        else if (timeSpan.Days == 0)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
        else
        {
            return timeSpan.Days + " days " + timeSpan.Hours + " hours";
        }
    }

    public static string GetStringTime1(double remainSeconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Convert.ToDouble(remainSeconds));
        if (timeSpan.Days == 0)
        {
            return timeSpan.ToString(@"hh\:mm\:ss");
        }
        else
        {
            return timeSpan.Days + " days " + timeSpan.Hours + " hours";
        }
    }

    public bool IsWeekend()
    {
        DayOfWeek day = DateTime.Now.DayOfWeek;

        if (day == DayOfWeek.Saturday
            || day == DayOfWeek.Sunday)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

[System.Serializable]
public class ScheduleTask
{
    // Persistent attribute
    public ScheduleID id;
    public long startTime;
    public bool started;
    public int usedPeriodCount;
    public double onlineSeconds;

    // Not Persistent attribute
    [NonSerialized]
    private DateTime? __startTime;
    [field: NonSerialized]
    private DateTime? _startTime
    {
        get
        {
            if (!__startTime.HasValue)
            {
                __startTime = new DateTime(startTime);
            }

            return __startTime;
        }
    }

    [field: NonSerialized]
    public int achievedPeriodCount
    {
        get
        {
            return (int)(runSeconds / data.onePeriodSeconds);
        }
    }

    [NonSerialized]
    private ScheduleData _data;
    [field: NonSerialized]
    public ScheduleData data
    {
        get
        {
            if (_data == null)
            {
                _data = TimeSchedule.Instance.GetScheduleData(id);
            }

            return _data;
        }
    }

    [field: NonSerialized]
    public double runSeconds
    {
        get
        {
            if (started == false)
            {
                return 0f;
            }

            double ret = (TimeSchedule.GetTime().Value - _startTime.Value).TotalSeconds;
            if (ret >= data.totalPeriodSeconds)
            {
                ret = data.totalPeriodSeconds;
            }
            return ret;
        }
    }

    [field: NonSerialized]
    public double remainSeconds
    {
        get
        {
            if (forceFinish)
            {
                return 0;
            }

            if (started == false)
            {
                return data.totalPeriodSeconds;
            }

            if (data.isFinishWhenNewDay)
            {
                DateTime now = TimeSchedule.GetTime().Value;
                if (now.Year > _startTime.Value.Year || now.DayOfYear > _startTime.Value.DayOfYear)
                {
                    return 0;
                }
                else
                {
                    return 86400 - now.TimeOfDay.TotalSeconds;
                }
            }
            else
            {
                double ret = data.totalPeriodSeconds - runSeconds;
                if (ret < 0)
                {
                    ret = 0;
                }
                return ret;
            }
        }
    }

    [NonSerialized]
    private bool _forceFinish;
    [field: NonSerialized]
    public bool forceFinish
    {
        get
        {
            return _forceFinish;
        }

        set
        {
            _forceFinish = value;
        }
    }

    [field: NonSerialized]
    public bool IsFinished
    {
        get
        {
            return started == false || remainSeconds <= 0;
        }
    }

    private void Init()
    {
        __startTime = TimeSchedule.GetTime();
        startTime = __startTime.Value.Ticks;
        usedPeriodCount = 0;
        forceFinish = false;
        onlineSeconds = 0f;
        started = data.autoStart;
    }

    public ScheduleTask(ScheduleID _id)
    {
        id = _id;
        Init();

        forceFinish = data.forceFinishFirstTime;
    }

    public void Start()
    {
        Init();
        started = true;
        TimeSchedule.Instance.Save();
    }

    public void Reset()
    {
        Init();
        TimeSchedule.Instance.Save();
    }

    public void SetStartTime(DateTime dateTime)
    {
        __startTime = dateTime;
        startTime = __startTime.Value.Ticks;
    }
}

[System.Serializable]
public class ScheduleData
{
    public ScheduleID id;
    public string strPeriod;        // days:hours:minutes:seconds
    public TimeSpan totalPeriod;    // Tổng thời gian của schedule = timeUpCount * onePeriod
    public int timeUpCount;
    public bool forceFinishFirstTime; // Lần đầu cho finish luôn
    public bool autoStart;
    public bool isFinishWhenNewDay;
    public bool onlyOnline; // Chỉ chạy khi online
    public double totalPeriodSeconds { get; set; }
    public double onePeriodSeconds { get; set; }

    public void Init()
    {
        List<int> datas = strPeriod.Split(':').Select(x =>
        {
            int ret;
            int.TryParse(x, out ret);
            return ret;
        }).ToList();

        totalPeriod = new TimeSpan(datas[0], datas[1], datas[2], datas[3]);
        totalPeriodSeconds = totalPeriod.TotalSeconds;
        onePeriodSeconds = totalPeriodSeconds / timeUpCount;
    }
}

public enum ScheduleID
{
    None = 0,
    DailyReward = 1,
    AdsWatchToday = 2,
    AdsWatchDelay = 3,
    FreeSpinDeLay = 4,

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeCoutDownDaily : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textTime;
    [SerializeField]
    ScheduleTask scheduleTask;
    private void OnEnable()
    {
        scheduleTask = TimeSchedule.Instance.GetScheduleTask(ScheduleID.DailyReward);
    }
    private void Update()
    {
        textTime.text = TimeSchedule.GetStringTime1(scheduleTask.remainSeconds);
    }
}

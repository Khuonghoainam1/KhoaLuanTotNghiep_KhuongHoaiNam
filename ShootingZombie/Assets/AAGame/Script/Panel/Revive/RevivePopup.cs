using System.Collections;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class RevivePopup : Popup
{
    public int timeCount;
    public TMP_Text timeText;
    public Button revive;
    public Image imgfill;
    private void OnEnable()
    {
        revive.onClick.RemoveListener(Revive);
        revive.onClick.AddListener(Revive);
    }

    public override void OnShow()
    {
        base.OnShow();
        SetUp();
    }

    public void SetUp()
    {
        timeCount = 10;
        imgfill.fillAmount = 1f;
        StartCoroutine(Countdown());
        
    }

    public void Revive()
    {
        base.Close();
        GameEvent.OnRevive.Invoke();
    }

    public override void Close()
    {
        base.Close();
        GameEvent.OnCloseRevive.Invoke();
    }

    IEnumerator Countdown()
    {
        timeText.text = timeCount.ToString();

        imgfill.DOFillAmount(0f, 10f);
        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount -= 1;
            timeText.text = timeCount.ToString();
            if (timeCount <= 0)
            {
                this.Close();
            }
         
        }
    }
}

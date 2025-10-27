using DanielLochner.Assets.SimpleScrollSnap;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class PopupSelectMode : Popup
{
    public Button playBossModeBtn;
    public Button playEndlessModeBtn;
    public Button playCollectionModeBtn;
    public GameObject TxtWarning;
    public Image[] imgNotUnlock;
    public Image[] imglock;
    public SkeletonGraphic[] SkeleletonPlay;
    public TMP_Text[] txtTypeLock;
    public SimpleScrollSnap scrollSnap;

    public void OnEnable()
    {
        GameEvent.OnMoveToPlay.RemoveListener(OnMoveToPlay);
        GameEvent.OnMoveToPlay.AddListener(OnMoveToPlay);
        for (int i = 0; i < imgNotUnlock.Length; i++)
        {
            if (User.Instance[ItemID.PlayingLevel] < 6)
            {
                // imgNotUnlock[i].color = new Color32(10, 10, 10, 100);
                //bgRoot.color = new Color32(10, 10, 10, 100);
                imglock[i].gameObject.SetActive(true);
                txtTypeLock[i].text = "UNLOCK";
            }
            else
            {
                imgNotUnlock[i].color = new Color(255f, 255f, 255f);
                imgNotUnlock[i].material = null;
                imglock[i].gameObject.SetActive(false);
                txtTypeLock[i].text = "PLAY";
            }

        }

        //for (int i = 0; i < SkeleletonPlay.Length; i++)
        //{
        //    if (User.Instance[ItemID.PlayingLevel] < 6)
        //    {
        //        //SkeleletonPlay[i].AnimationState.SetAnimation(0, "0_blue", true);
        //        /// SkeleletonPlay[i].Skeleton.SetSlotsToSetupPose();

        //    }
        //    else
        //    {
        //        /* SkeleletonPlay[i].AnimationState.SetAnimation(0, "0_yellow", true);
        //         SkeleletonPlay[i].Skeleton.SetSlotsToSetupPose(); */

        //    }

        //}
    }
    public void OnMoveToPlay()
    {
        this.Close();
    }

    public void OpenSelectBoss()
    {
        AudioManager.instance.Play("BtnClick");
        GlobalData.gameMode = GameMode.BossWorld;
        Debug.Log(User.Instance[ItemID.PlayingLevel]);
        User.Instance.Save();
        PopupManager.Instance.OpenPopup<PopupSelectBoss>(PopupID.PopupSelectBoss);
        /*if (User.Instance[ItemID.PlayingLevel] > 5)
        {
            GlobalData.gameMode = GameMode.BossWorld;
            Debug.Log(User.Instance[ItemID.PlayingLevel]);
            User.Instance.Save();
            PopupManager.Instance.OpenPopup<PopupSelectBoss>(PopupID.PopupSelectBoss);
        }
        else
        {
            StartCoroutine(clicbtnLock());
        }*/

    }
    IEnumerator clicbtnLock()
    {
        TxtWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        TxtWarning.gameObject.SetActive(false);
    }

    public void OpenCollectFuel()
    {
        //GlobalData.gameMode = GameMode.CollectFuel;
        //User.Instance.Save();
        //Debug.Log(User.Instance[ItemID.PlayingLevel]);

        //PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) =>
        //{
        //    pop.Items[0].gameObject.SetActive(true);
        //    pop.Items[1].gameObject.SetActive(false);
        //    pop.Items[2].gameObject.SetActive(false);
        //    pop.SetData(2);
        //});
        AudioManager.instance.Play("BtnClick");
        if (User.Instance[ItemID.PlayingLevel] > 5)
        {
            GlobalData.gameMode = GameMode.CollectFuel;
            User.Instance.Save();
            Debug.Log(User.Instance[ItemID.PlayingLevel]);

            PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) =>
            {
                pop.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                pop.Items[0].gameObject.SetActive(true);
                pop.Items[1].gameObject.SetActive(false);
                pop.Items[2].gameObject.SetActive(false);
                pop.txtReview.gameObject.SetActive(true);
                pop.txtGold.gameObject.SetActive(false);
                pop.txtTiket.gameObject.SetActive(false);
                pop.txtReview.text = "Kill zombies to earn gold";
                pop.ClickPlay.gameObject.SetActive(true);
                pop.txtboss.gameObject.SetActive(false);
                pop.SetData(2);
            });
        }
        else
        {
            StartCoroutine(clicbtnLock());

        }

    }

    public void PlayModeEndLess()
    {

        //GlobalData.gameMode = GameMode.Endless;
        //Debug.Log(User.Instance[ItemID.PlayingLevel]);

        //User.Instance.Save();
        //PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) =>
        //{
        //    pop.Items[0].gameObject.SetActive(true);
        //    pop.Items[1].gameObject.SetActive(false);
        //    pop.Items[2].gameObject.SetActive(false);
        //    pop.SetData(1);
        //});
        AudioManager.instance.Play("BtnClick");
        if (User.Instance[ItemID.PlayingLevel] > 5)
        {

            GlobalData.gameMode = GameMode.Endless;
            Debug.Log(User.Instance[ItemID.PlayingLevel]);

            User.Instance.Save();
            PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) =>
            {
                pop.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                pop.Items[0].gameObject.SetActive(true);
                pop.Items[1].gameObject.SetActive(false);
                pop.Items[2].gameObject.SetActive(false);
                pop.txtReview.gameObject.SetActive(true);
                pop.txtReview.text = "Kill zombies to receive items";
                pop.txtGold.gameObject.SetActive(false);
                pop.txtTiket.gameObject.SetActive(false);
                pop.ClickPlay.gameObject.SetActive(true);
                pop.txtboss.gameObject.SetActive(false);
                pop.SetData(1);
            });
        }
        else
        {
            StartCoroutine(clicbtnLock());

        }

    }


    public void PlayModeNormalMode()
    {
        AudioManager.instance.Play("BtnClick");
        GlobalData.instance.levelToPlay = User.Instance[ItemID.PlayingLevel];
        GlobalData.gameMode = GameMode.Normal;
        Debug.Log(User.Instance[ItemID.PlayingLevel]);
        GameEvent.OnMoveToPlay.Invoke();
        User.Instance.Save();
        /* PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) =>
         {
             pop.Items[0].gameObject.SetActive(true);
             pop.Items[1].gameObject.SetActive(false);
             pop.Items[2].gameObject.SetActive(false);
             pop.SetData(1);
         });*/



    }
}

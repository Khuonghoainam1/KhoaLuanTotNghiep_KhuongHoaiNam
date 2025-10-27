using AA_Game;
using Gamelogic.Extensions.Algorithms;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.GameCore;

public class PopupReviewReward : Popup
{
    public Button ClickPlay;
    public List<CellReview> Items = new List<CellReview>();
    public List<GameObject> iconGameObject = new List<GameObject>();
    public int index;
    public Button GameojctLock;
    public GameObject TxtWarning;
    
    public SkeletonGraphic SkinIcon;
    public TMP_Text txtReview;
    public TMP_Text txtboss;
    public TMP_Text txtGold;
    public TMP_Text txtTiket;
    public void ExitPopup()
    {
        base.Close();

    }
    public void OnEnable()
    {
     /*   if (User.Instance[ItemID.PlayingLevel] >6 ) 
        {
            GameojctLock.gameObject.SetActive(false);
            ClickPlay.interactable = true;
        }
        else if (User.Instance[ItemID.PlayingLevel] <= 6)
        {
            GameojctLock.gameObject.SetActive(true);
            ClickPlay.interactable = false;
        }*/
       // SkinIcon = new SkeletonGraphic();
        //SetData();
    }
    
    public void SetData(int id)
    {
        iconGameObject[id].gameObject.SetActive(true);
      
        ClickPlay.onClick.RemoveListener(ClickPlayRevew);
        ClickPlay.onClick.AddListener(ClickPlayRevew);
     
        /*  GameojctLock.onClick.RemoveListener(BtnLock);
          GameojctLock.onClick.AddListener(BtnLock);*/


    }
    public override void Close()
    {
        base.Close();
        for (int i = 0; i < iconGameObject.Count; i++)
        {
            iconGameObject[i].gameObject.SetActive(false);

        }
       
    }
    public void ClickPlayRevew()
    {
      
        GameEvent.OnMoveToPlay.Invoke();
     
        this.Close();
    }
    void BtnLock()
    {
        StartCoroutine(clicbtnLock());
    }
    IEnumerator clicbtnLock()
    {
        TxtWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        TxtWarning.gameObject.SetActive(false);
    }
}

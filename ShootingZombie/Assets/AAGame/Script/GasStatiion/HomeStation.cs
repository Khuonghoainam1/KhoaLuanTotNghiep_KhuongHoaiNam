using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Thanh.Core;
using System.Collections;
using Gamelogic.Extensions.Obsolete;

public class HomeStation : MonoBehaviour
{
    public int level;
    public List<GameObject> itemDecor = new List<GameObject>();
    public GameObject beNangXe;
    public SpriteRenderer nenNha;
    public List<Transform> transDecor = new List<Transform>();

    private void OnEnable()
    {
        GameEvent.OnUpgradeGara.RemoveListener(SetData);
        GameEvent.OnUpgradeGara.AddListener(SetData);


        GameEvent.OnMoveToPlay.RemoveListener(OnMoveToPlay);
        GameEvent.OnMoveToPlay.AddListener(OnMoveToPlay);


        Invoke("SetData", 1f);
        //SetData();
    }

    public void SetData()
    {
        level = User.Instance[ItemID.levelHomeStation];

        Debug.Log(level);
        /*if (level >= 2)
        {*/
        int x = 0;
        for (int i = 0; i < level - 1; i++)
        {
            itemDecor[i].SetActive(true);
            x = i;
            //GameManager.Instance.TurnArrow(itemDecor[i + 1].transform);


            // GameManager.Instance.TurnArrow(itemDecor[i].transform);
        }
        Debug.Log(User.Instance[ItemID.levelHomeStation]-1);
        if (User.Instance[ItemID.levelHomeStation] <5 && User.Instance[ItemID.levelHomeStation] >=1)
        {
            GameManager.Instance.TurnArrow(itemDecor[User.Instance[ItemID.levelHomeStation] - 1].transform);
           
        }
        //  }
        if (User.Instance[ItemID.levelHomeStation] == 5)
        {
            GameScene.main.homePanel.upgradeHomeStationBtn.gameObject.SetActive(false);
        }  
       
       
    }


    public void OnMoveToPlay()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return new WaitForSeconds(2f);
        GameManager.Instance.trainManager.transform.parent = beNangXe.transform;
        GameManager.Instance.cam.GetComponent<CameraFollow>().enabled = false;
        nenNha.SetSortingLayer(SortingLayerID.Character, 500);
        beNangXe.transform.DOLocalMoveY(-6f, 2f);
        yield return new WaitForSeconds(1.5f);
        nenNha.SetSortingLayer(SortingLayerID.Character, 0);
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
    }
}

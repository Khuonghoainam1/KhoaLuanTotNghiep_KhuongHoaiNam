using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class PopupDefeat2 : Popup
{
    public Button btnReplay;
    public Button btnUpgrade;
    public Transform groupItemDrop;
    public Button btnClaim;
    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;

    public void OnEnable()
    {
        //btnHome.onClick.RemoveListener(c)
        btnUpgrade.onClick.RemoveListener(ClickUpgrade);
        btnUpgrade.onClick.AddListener(ClickUpgrade);

        btnReplay.onClick.RemoveListener(Retry);
        btnReplay.onClick.AddListener(Retry);

        //btnClaim.onClick.RemoveListener(Clickclaim);
        //btnClaim.onClick.AddListener(Clickclaim);
    }


    public void Clickclaim()
    {
        CountCoins();
    }
    public override void OnShow()
    {
        base.OnShow();
        //AudioAssistant.PlaySound("loser");
        foreach (Transform items in groupItemDrop)
        {
            if(items != null)
            {
                Destroy(items.gameObject);
            }
        }
        ShowItemDrop();
        StartCoin();
    }
    public override void Close()
    {
        base.Close();
        GlobalData.gameMode = GameMode.Home;
        User.Instance.Save();
       // stopArow = true;
        AudioAssistant.main.StopSound();
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());

    }
    private void ClickUpgrade()
    {
        //base.Close();
        //User.Instance.openUpdate = true;
        //User.Instance.Save();
        GlobalData.instance.isOpenUpgrade = true;
        Close();
    }
    public void Retry()
    {
        // base.Close();
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
    }
 
    public void ShowItemDrop()
    {

        //gold
        if (GameManager.Instance.goldReward > 0)
        {
            GameObject coinDropUI = Instantiate(Resources.Load<GameObject>("ItemDrop/CoinDrop"), groupItemDrop);
            coinDropUI.GetComponent<ItemDropUI>().SetUp("coin", GameManager.Instance.goldReward,true);
        }

        //item dot pha
        if (GameManager.Instance.itemDotPha > 0)
        {
            GameObject itemDotPhaDropUI = Instantiate(Resources.Load<GameObject>("ItemDrop/CoinDrop"), groupItemDrop);
            itemDotPhaDropUI.GetComponent<ItemDropUI>().SetUp("itemDotPha", GameManager.Instance.itemDotPha);
        }


        //bua
        if (GameManager.Instance.bua > 0)
        {
            GameObject buaUI = Instantiate(Resources.Load<GameObject>("ItemDrop/CoinDrop"), groupItemDrop);
            buaUI.GetComponent<ItemDropUI>().SetUp("hammer", GameManager.Instance.bua);
        }

        //ticket
        /*if (GameManager.Instance.ticketReward > 0)
        {
            GameObject ticketDropUI = Instantiate(Resources.Load<GameObject>("ItemDrop/TicketDrop"), groupItemDrop);
            ticketDropUI.GetComponent<ItemDropUI>().SetUp("ticket", GameManager.Instance.ticketReward);
        }*/

        ////item
        //foreach (ItemDrop item in GameManager.Instance.listItemDrop)
        //{
        //    GameObject itemDropUI = Instantiate(Resources.Load<GameObject>("ItemDrop/ItemDropUI"), groupItemDrop);
        //    itemDropUI.GetComponent<ItemDropUI>().SetUp(item.IDManhSung.ToString(), item.amount);
        //}
    }

    public void StartCoin()
    {
        if (coinsAmount == 0)
            coinsAmount = 10; // you need to change this value based on the number of coins in the inspector
        pileOfCoins.gameObject.SetActive(true);
        initialPos = new Vector2[coinsAmount];
        initialRotation = new Quaternion[coinsAmount];
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            initialPos[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition;
            initialRotation[i] = pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation;
        }

    }

    public void resetCoin()
    {
        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).gameObject.SetActive(false);
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = initialPos[i];
            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().rotation = initialRotation[i];
        }
    }
    public void CountCoins()
    {
        pileOfCoins.SetActive(true);

        var delay = 0f;

        for (int i = 0; i < pileOfCoins.transform.childCount; i++)
        {
            pileOfCoins.transform.GetChild(i).gameObject.SetActive(true);
            pileOfCoins.transform.GetChild(i).DOScale(1f, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);

            pileOfCoins.transform.GetChild(i).GetComponent<RectTransform>().DOAnchorPos(new Vector2(808f, 465f), 0.8f)
                .SetDelay(delay + 0.5f).SetEase(Ease.InBack);


            pileOfCoins.transform.GetChild(i).DORotate(Vector3.zero, 1f).SetDelay(delay + 1f)
                .SetEase(Ease.Flash).OnComplete(() => resetCoin());


            // pileOfCoins.transform.GetChild(i).DOScale(0f, 0.3f).SetDelay(delay + 1.5f).SetEase(Ease.OutBack);

            delay += 0.1f;
            //Debug.Log(pileOfCoins.transform) ;
            //  pileOfCoins.gameObject.SetActive(false);
        }


    }
}

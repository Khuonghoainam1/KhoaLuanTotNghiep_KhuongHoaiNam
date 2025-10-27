using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thanh.Core;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class PopUpGameWin : Popup
{
    public TextMeshProUGUI txtEarn;
    public RectTransform arrow;
    public List<TextMeshProUGUI> cellTxts;
    public List<int> muls;
    public Button btnClaim;
    public Button btnNoThanks;
    public Button btnRevive;
    public TextMeshProUGUI txtReward;
    [SerializeField]
    private Image title;
    public Image bgSpin;
    public Transform groupItemDrop;
    public List<Image> imgStarNomal;
    public Sprite imgStarTrue;
    public TMP_Text txtGold;
    public TMP_Text txtThor;
    int rewardGold;
    int rewardItemDotPha;
    int mulReward;
    int mulitemDotPha;
    int _spinIdx;
    int rewardTicket;

    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount ;

    private void OnValidate()
    {
        muls = new List<int>();
        foreach (TextMeshProUGUI text in cellTxts)
        {
            muls.Add(int.Parse(text.text.Replace("x", "")));
        }
    }

    /// <summary>
    /// Show star by star amount from GameManager
    /// </summary>
    public void SetStarLevel()
    {
        foreach(Image img in imgStarNomal)
        {
            img.gameObject.SetActive(false);
        }
        for (int i = 0; i <3 /* GameManager.Instance.starAmount*/; i++)
        {
            imgStarNomal[i].gameObject.SetActive(true);
        }
    }



    public void SetData()
    {
        rewardGold = GameManager.Instance.goldReward;
        rewardItemDotPha = GameManager.Instance.itemDotPha;
        rewardTicket = GameManager.Instance.ticketReward;
        stopArow = false;
        btnClaim.interactable = true;
        btnClaim.onClick.RemoveListener(OnClaimClick);
        btnClaim.onClick.AddListener(OnClaimClick);

        btnNoThanks.gameObject.SetActive(false);
        btnNoThanks.onClick.RemoveListener(OnNoThanks);
        btnNoThanks.onClick.AddListener(OnNoThanks);
    }


    public override void OnShow()
    {
        base.OnShow();
       
       // AudioAssistant.PlaySoundWhile("LuckySpin", () => !stopArow);
        SetData();
        ShowItemDrop();
        StartCoroutine(IE_NoThanks());
        SetStarLevel();
        StartCoin();
        GameScene.main.popupPlaying.goldInfo.gameObject.SetActive(false); ;

}


[SerializeField]
    private Transform arowSpin;
    [SerializeField]
    private float pointLeft, pointRight, pointCurrent;
    //Speed goc va speed hien tai ,speed thay doi 
    private float speed = 250;
    private float speedCurrent = 900;
    private float speedChangSpeedCurrent = 1000;
    //huong cua arow
    private float vectorMove = 1;
    private bool stopArow = false;
    private void Update()
    {
        if (stopArow == true)
        {
            return;
        }
        if (pointCurrent >= pointRight)
        {
            // doi chieu di
            vectorMove = -1;
            speedCurrent = speed;
        }
        if (pointCurrent <= pointLeft)
        {
            // doi chieu di
            vectorMove = 1;
            speedCurrent = speed;
        }
        if (vectorMove == -1)
        {
            if (pointCurrent > (pointRight - pointLeft) / 2)
            {
                speedCurrent += Time.deltaTime * speedChangSpeedCurrent;
            }
            if (pointCurrent < (pointRight - pointLeft) / 2 && speedCurrent > speed)
            {
                speedCurrent -= Time.deltaTime * speedChangSpeedCurrent;
            }
        }
        if (vectorMove == 1)
        {
            if (pointCurrent < (pointRight - pointLeft) / 2)
            {
                speedCurrent += Time.deltaTime * speedChangSpeedCurrent;
            }
            if (pointCurrent > (pointRight - pointLeft) / 2 && speedCurrent > speed)
            {
                speedCurrent -= Time.deltaTime * speedChangSpeedCurrent;
            }
        }
        arowSpin.localPosition += new Vector3(vectorMove, 0, 0) * Time.deltaTime * speedCurrent;
        pointCurrent = arowSpin.localPosition.x;
        spinIdx = Mathf.Clamp(((int)(pointCurrent / ((pointRight - pointLeft) / cellTxts.Count))), 0, 4);
    }

    public List<Transform> listBox;

    int spinIdx
    {
        get
        {
            return _spinIdx;
        }

        set
        {
            if (_spinIdx == value)
            {
                return;
            }
            _spinIdx = value;
            mulReward = rewardGold * muls[_spinIdx];
            mulitemDotPha = rewardItemDotPha * muls[_spinIdx];

            GameManager.Instance.numberMul = (int)muls[_spinIdx];


            txtReward.text = mulReward.ToKMB();
            listBox.ForEach(x => x.localScale = Vector3.one);
            listBox[_spinIdx].localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }

    IEnumerator IE_NoThanks()
    {
        yield return WaitForSecondsCache.Get(2f);
        btnNoThanks.gameObject.SetActive(true);
        btnNoThanks.interactable = true;
    }

    void OnNoThanks()
    {
        btnNoThanks.interactable = false;
        btnClaim.interactable = false;
        AudioManager.instance.Play("BtnClick");
        OnClaim(rewardGold,rewardItemDotPha, rewardTicket, btnNoThanks.transform.position);
        GameManager.Instance.SaveItemDrop(1);
        //User.Instance[ItemID.ticket] += GameManager.Instance.ticketReward;
        // AdsManager.Instance.TryToShowInterstitial("spin_no_thank_win", null, null);
    }



    void OnClaimClick()
    {
        btnClaim.interactable = false;
        btnNoThanks.interactable = false;
        AudioManager.instance.Play("BtnClick");
        stopArow = true;
        GameManager.Instance.SaveItemDrop((int)muls[_spinIdx]);
        //User.Instance[ItemID.ticket] += GameManager.Instance.ticketReward * muls[_spinIdx];

        //khi đang chơi leve 4 thì tut mode collect
        #region
        /* if (User.Instance[ItemID.PlayingLevel] == 2)
         {
             User.Instance.IndexBtnTutData = 4;
         }*/
        #endregion
        User.Instance[ItemID.TutPlay] = 4;
        //BuyManager.Instance.Buy(new List<ItemValueFloat> { new ItemValueFloat(ItemID.Ads, 1) }, null, isSuccess =>
        //{
        //    if (isSuccess)
        //    {
        //        OnClaim(mulReward, btnClaim.transform.position);
        //    }
        //    else
        //    {
        //        btnClaim.interactable = true;
        //        stopArow = false;
        //    }

        //}, AdLocation.EndGame);
        OnClaim(mulReward,rewardItemDotPha, rewardTicket, btnClaim.transform.position);
    }

    void OnClaim(int claimGold, int itemDotPha,int ticket, Vector3 pos)
    {
        //BuyManager.ClaimFlyItem(new ItemValue(ItemID.Gold, claimGold), pos, AdLocation.EndGame, null);
        User.Instance[ItemID.Gold] += claimGold;
        User.Instance[ItemID.itemDotPha] += itemDotPha;
        User.Instance[ItemID.ticket] += ticket;
        StartCoroutine(OnClaimClosePopup());
        CountCoins();
    }

    IEnumerator OnClaimClosePopup()
    {
        yield return WaitForSecondsCache.Get(3f);
        Close();
    }

    public override void Close()
    {
        base.Close();
        GlobalData.gameMode = GameMode.Home;
        User.Instance.Save();
        stopArow = true;
        AudioAssistant.main.StopSound();
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
        foreach (Transform item in groupItemDrop)
        {
            Destroy(item.gameObject);
        }
    }

    public void ShowItemDrop()
    {
        //gold
        if (GameManager.Instance.goldReward > 0)
        {
            GameObject coinDropUI = Instantiate(Resources.Load<GameObject>("ItemDrop/CoinDrop"), groupItemDrop);
            coinDropUI.GetComponent<ItemDropUI>().SetUp("coin", GameManager.Instance.goldReward,true);
        }

        //ticket
        if(GameManager.Instance.ticketReward > 0)
        {
            GameObject ticketDropUI = Instantiate(Resources.Load<GameObject>("ItemDrop/TicketDrop"), groupItemDrop);
            ticketDropUI.GetComponent<ItemDropUI>().SetUp("ticket", GameManager.Instance.ticketReward);
        }

        //item dot pha
        if (GameManager.Instance.itemDotPha > 0)
        {
            GameObject itemDotPhaDropUI = Instantiate(Resources.Load<GameObject>("ItemDrop/CoinDrop"), groupItemDrop);
            itemDotPhaDropUI.GetComponent<ItemDropUI>().SetUp("itemDotPha", GameManager.Instance.itemDotPha);
        }

        ////manh sung
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

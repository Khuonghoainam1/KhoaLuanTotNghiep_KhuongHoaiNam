using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopUpGameDefect : Popup
{
    public TextMeshProUGUI txtEarn;
    public RectTransform arrow;
    public List<TextMeshProUGUI> cellTxts;
    public List<int> muls;
    public Button btnClaim;
    public Button btnNoThanks;
    public Button btnRevive;
    public Button btnReplay;
    public TextMeshProUGUI txtReward;
    [SerializeField]
    private Image title;
    public Image bgSpin;

    public Transform groupItemDrop;
    [SerializeField]
    private Button Upgrdate;
    int rewardGold;
    int mulReward;

    int rewardItemDotPha;
    int mulitemDotPha;

    int rewardBua;
    int mulBua;

    int _spinIdx;

    [SerializeField] private GameObject pileOfCoins;
    [SerializeField] private TextMeshProUGUI counter;
    [SerializeField] private Vector2[] initialPos;
    [SerializeField] private Quaternion[] initialRotation;
    [SerializeField] private int coinsAmount;
    private void OnValidate()
    {
        muls = new List<int>();
        foreach (TextMeshProUGUI text in cellTxts)
        {
            muls.Add(int.Parse(text.text.Replace("x", "")));
        }
    }

    bool isWin;

    public void SetData()
    {
        stopArow = false;
        rewardGold = GameManager.Instance.goldReward;
        rewardItemDotPha = GameManager.Instance.itemDotPha;
        rewardBua = GameManager.Instance.bua;
        //txtEarn.text = rewardGold.ToKMB();
        //title.SetNativeSize();
        btnClaim.interactable = true;
        btnClaim.onClick.RemoveListener(OnClaimClick);
        btnClaim.onClick.AddListener(OnClaimClick);

        btnNoThanks.gameObject.SetActive(false);
        btnNoThanks.onClick.RemoveListener(OnNoThanks);
        btnNoThanks.onClick.AddListener(OnNoThanks);

        /*Upgrdate.onClick.RemoveListener(ClickUpgrade);
        Upgrdate.onClick.AddListener(ClickUpgrade);*/

        btnReplay.onClick.RemoveListener(ClickRepPlay);
        btnReplay.onClick.AddListener(ClickRepPlay);
        //btnRevive.onClick.RemoveAllListeners();
        //btnRevive.onClick.AddListener(Revive);

    }
    public Image bgLoseImgage;
    public override void Close()
    {
        base.Close();
        foreach (Transform items in groupItemDrop)
        {
            Destroy(items.gameObject);
        }
    }


    public override void OnShow()
    {
        base.OnShow();
        //AudioAssistant.PlaySoundWhile("LuckySpin", () => !stopArow);
        //AudioAssistant.PlaySound("loser");
       
        SetData();
        StartCoroutine(IE_NoThanks());
        ShowItemDrop();
        StartCoin();
        GameScene.main.popupPlaying.goldInfo.gameObject.SetActive(false); 

    }

    void OnPlay()
    {
        // AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");
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

            mulReward = GameManager.Instance.goldReward * muls[_spinIdx];
            mulitemDotPha = GameManager.Instance.itemDotPha * muls[_spinIdx];
            mulBua = GameManager.Instance.bua * muls[_spinIdx];

            GameManager.Instance.numberMul = (int)muls[_spinIdx];

            txtReward.text = mulReward.ToKMB();
            listBox.ForEach(x => x.localScale = Vector3.one);
            listBox[_spinIdx].localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
    }

    float rotateSpeed = 400f;
    IEnumerator IE_NoThanks()
    {
        yield return WaitForSecondsCache.Get(2f);
        btnNoThanks.gameObject.SetActive(true);
        btnNoThanks.interactable = true;
    }

    void OnNoThanks()
    {
        /* btnNoThanks.interactable = false;
         btnClaim.interactable = false;

         //AudioAssistant.PlaySound("BtnClick");
         AudioManager.instance.Play("BtnClick");
         OnClaim(rewardGold, btnNoThanks.transform.position);
         GameManager.Instance.SaveItemDrop(1);*/
        mulReward = 1;
        GameManager.Instance.numberMul = mulReward;

        OnClaim(rewardGold,rewardItemDotPha,rewardBua, btnNoThanks.transform.position);
        Close();
        PopupManager.Instance.OpenPopup<PopupDefeat2>(PopupID.PopupDefeat2);
        User.Instance.Save();
        stopArow = true;
        AudioAssistant.main.StopSound();
      
      
       
        // AdsManager.Instance.TryToShowInterstitial("spin_no_thank_win", null, null);
    }



    void OnClaimClick()
    {
        btnClaim.interactable = false;
        btnNoThanks.interactable = false;
        AudioManager.instance.Play("BtnClick");
        stopArow = true;
        //GameManager.Instance.SaveItemDrop((int)muls[_spinIdx]);
        //User.Instance[ItemID.ticket] += GameManager.Instance.ticketReward * muls[_spinIdx];
        CountCoins();

        //khi đang chơi leve 4 thì tut mode collect
        #region
        /* if (User.Instance[ItemID.PlayingLevel] == 2)
         {
             User.Instance.IndexBtnTutData = 4;
         }*/
        #endregion
        User.Instance[ItemID.TutPlay] = 4;
        #region
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
        #endregion
        OnClaim(mulReward,rewardItemDotPha,rewardBua, btnClaim.transform.position);
        foreach (Transform items in groupItemDrop)
        {
            Destroy(items.gameObject);
        }
    }

    void OnClaim(int claimGold,int itemDotPha,int bua, Vector3 pos)
    {
        //BuyManager.ClaimFlyItem(new ItemValue(ItemID.Gold, claimGold), pos, AdLocation.EndGame, null);
        User.Instance[ItemID.Gold] += claimGold;
        User.Instance[ItemID.itemDotPha] += itemDotPha;
        User.Instance[ItemID.thorAmount] += bua;
        StartCoroutine(OnClaimClosePopup());
    }

    IEnumerator OnClaimClosePopup()
    {
        yield return WaitForSecondsCache.Get(3f);
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());

        GlobalData.gameMode = GameMode.Home;
        Close();

    }
  
    /* public override void Close()
     {
         base.Close();
         GlobalData.gameMode = GameMode.Home;
         User.Instance.Save();
         stopArow = true;
         AudioAssistant.main.StopSound();
         foreach (Transform items in groupItemDrop)
         {
             Destroy(items.gameObject);
         }
         Loader.Instance.LoadScene(SceneName.GameScene.ToString());

     }*/

    //public void Revive()
    //{
    //    GameManager.Instance.Revive();
    //    base.Close();
    //}

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
        #region
        //ticket
        /* if (GameManager.Instance.ticketReward > 0)
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
        #endregion
    }

    private void ClickRepPlay()
    {
        if (GlobalData.gameMode == GameMode.Normal)
        {
            //Loader.Instance.LoadScene(SceneName.GameScene.ToString());
            
        }
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

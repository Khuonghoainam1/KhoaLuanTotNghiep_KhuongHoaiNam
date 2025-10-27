using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupEndGame : Popup
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

    int rewardGold;
    int mulReward;
    int _spinIdx;
    private void OnValidate()
    {
        muls = new List<int>();
        foreach (TextMeshProUGUI text in cellTxts)
        {
            muls.Add(int.Parse(text.text.Replace("x", "")));
        }
    }

    bool isWin;

    public void SetData(bool _isWin, int _rewardGold,int star)
    {
        isWin = _isWin;
        stopArow = false;
        rewardGold = isWin ? _rewardGold : _rewardGold / 2;
        //txtEarn.text = rewardGold.ToKMB();
        //title.SetNativeSize();
        btnClaim.interactable = true;
        btnClaim.onClick.RemoveAllListeners();
        btnClaim.onClick.AddListener(OnClaimClick);
       // btnNoThanks.interactable = true;
        btnNoThanks.gameObject.SetActive(false);
        btnNoThanks.onClick.RemoveAllListeners();
        btnNoThanks.onClick.AddListener(OnNoThanks);
        btnRevive.onClick.RemoveAllListeners();
        //btnRevive.onClick.AddListener(Revive);
 
    }
    public Image bgLoseImgage;



    public override void OnShow()
    {
        base.OnShow();
        //AudioAssistant.PlaySoundWhile("LuckySpin", () => !stopArow);
        StartCoroutine(IE_NoThanks());
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
            mulReward = rewardGold * muls[_spinIdx];
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
    }

    void OnNoThanks()
    {
        btnNoThanks.interactable = false;
        btnClaim.interactable = false;

        //  AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");

        OnClaim(rewardGold, btnNoThanks.transform.position);

       // AdsManager.Instance.TryToShowInterstitial("spin_no_thank_win", null, null);
    }

    

    void OnClaimClick()
    {
        btnClaim.interactable = false;
        btnNoThanks.interactable = false;
        //AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");

        stopArow = true;

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
        OnClaim(mulReward, btnClaim.transform.position);
    }

    void OnClaim(int claimGold, Vector3 pos)
    {     
        //BuyManager.ClaimFlyItem(new ItemValue(ItemID.Gold, claimGold), pos, AdLocation.EndGame, null);
        User.Instance[ItemID.Gold] += claimGold;
        StartCoroutine(OnClaimClosePopup());
    }

    IEnumerator OnClaimClosePopup()
    {
        yield return WaitForSecondsCache.Get(3f);
        Close();
    }

    public override void Close()
    {
        base.Close();
        stopArow = true;
        AudioAssistant.main.StopSound();
        Loader.Instance.LoadScene(SceneName.GameScene.ToString());
    }

    //public void Revive()
    //{
    //    GameManager.Instance.Revive();
    //    base.Close();
    //}
}


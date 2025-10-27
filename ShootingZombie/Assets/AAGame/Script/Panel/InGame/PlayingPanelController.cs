using Thanh.Core;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;
using DG.Tweening;

public class PlayingPanelController : Popup
{
    public Button PauseButton;
    public Button winBtn;
    public Button lossBtn;
    public TMP_Text levelPlaying;
    public Transform parentBooster;
    public Button btnAutoPlay;
    public Button btnOpenBoosterList;
    public TMP_Text textTime;
    public GameObject bgTimePlay;

    public Skeleton skeleton; 
    public SkeletonGraphic  skeletonAnimation;

    public GameObject stageLable;
    // public Button btnSeting;
    public Slider sliderHpBar;
    public TMP_Text txtHealth;
    public GameObject goldInfo;
    public GameObject animPoin;

    public Transform[] listPosBooster;

    public TMP_Text txtReview;
    private void OnEnable()
    {
        PauseButton.onClick.RemoveListener(Pause);
        PauseButton.onClick.AddListener(Pause);
       
        winBtn.onClick.AddListener(() => GameManager.Instance.WinGame());
        lossBtn.onClick.AddListener(() => GameEvent.OnPlayerLose.Invoke());

        btnAutoPlay.onClick.RemoveListener(AutoPlayButton);
        btnAutoPlay.onClick.AddListener(AutoPlayButton);

        btnOpenBoosterList.onClick.RemoveListener(OpenBoosterList);
        btnOpenBoosterList.onClick.AddListener(OpenBoosterList);

        levelPlaying.text = GlobalData.instance.levelToPlay.ToString();

        StartCoroutine(ShowBooster());

        if(User.Instance[ItemID.TutPlay] == 4)
        {
            PauseButton.gameObject.SetActive(true);
        }
        else
        {
            PauseButton.gameObject.SetActive(false);
        }

        animPoin.gameObject.SetActive(true);

     //   ShowStage();
    }

    public void ShowStage()
    {
        stageLable.transform.DOLocalMoveX(0, 0.6f).From(3000).OnComplete(() =>
        {
            stageLable.transform.DOLocalMoveX(-3000, 0.6f).From(0).SetDelay(0.3f);
        });
    }

    public override void Close()
    {
        base.Close();
        foreach (Transform obj in parentBooster)
        {
            Destroy(obj.gameObject);
        }
    }

    public void Pause()
    {
        GameManager.Instance.PauseGame();
        //  GameScene.main.popupPause.OnShow();
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupSetting>(PopupID.PopupSetingGameplay);
    }

    IEnumerator ShowBooster()
    {
        yield return new WaitUntil(() => GameManager.Instance.isSelectBooster == true);
        yield return new WaitForSeconds(2f);


        if (User.Instance[ItemID.TutPlay] == 0)
        {
            yield return new WaitUntil(() => User.Instance[ItemID.TutPlay] == 1);
            yield return new WaitForSeconds(1f);
        }
        else
        {
            yield return new WaitUntil(() => (GameManager.Instance.enemiesCurrentAmount > 0));
        }

        for (int i =0; i < BoosterManager.instance.boosterVip.Count;i++)
        {
            if(GlobalData.gameMode != GameMode.CollectFuel)
            {
                GameObject boost = Instantiate(Resources.Load<GameObject>("UI/Booster/ButtonBoosterVip"), listPosBooster[i]);
                boost.GetComponent<BoosterButton>().SetUp(BoosterManager.instance.boosterVip[i], this.popupID);
            }
            else
            {
                if(BoosterManager.instance.boosterVip[i].booster != NameBooster.Shield)
                {
                    GameObject boost = Instantiate(Resources.Load<GameObject>("UI/Booster/ButtonBoosterVip"), listPosBooster[i]);
                    boost.GetComponent<BoosterButton>().SetUp(BoosterManager.instance.boosterVip[i], this.popupID);
                }
            }
        }
    }

    public void AutoPlayButton()
    {
        GameManager.Instance.isAutoPlay = true;
        GameEvent.OnShootingAuto.Invoke();
        btnAutoPlay.interactable = false;
        skeletonAnimation.AnimationState.SetAnimation(0, "auto_on", true);
    }
/*    public void OpenSetingOnGame()
    {
        
  
        GameManager.Instance.PauseGame();
        //GameScene.main.popupPause.OnShow();
    }*/
    public void OpenBoosterList()
    {
        AudioManager.instance.Play("BtnClick");
        PopupManager.Instance.OpenPopup<PopupBoosterUnchanged>(PopupID.PopupBoosterUnchanged);
    }
}

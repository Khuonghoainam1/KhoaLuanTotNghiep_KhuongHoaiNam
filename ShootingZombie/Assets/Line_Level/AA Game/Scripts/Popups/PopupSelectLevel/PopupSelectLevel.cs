using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.GameCore;

public class PopupSelectLevel : Popup
{
    [SerializeField] private Button btnPre, btnNext, btnCancel;
    [SerializeField] private int mapID;
    [SerializeField] private MapLevelUI mapLevelUICurrent;
    [SerializeField] private Transform mapTf;
    [SerializeField] private TextMeshProUGUI textGiftStar;
    [SerializeField] private int startneed;
    [SerializeField] private Image lineStar;
    [SerializeField] private List<MapLevelUI> mapLevelUIList;
    [SerializeField] private bool loadMap;

    private float widthScreen;
    private float flip;
    private bool isAnimPlay;

    private void Awake()
    {
        widthScreen = Screen.width;
        mapLevelUIList.ForEach(x => x.TurnOff());
    }

    public override void Start()
    {
        btnPre.onClick.RemoveAllListeners();
        btnPre.onClick.AddListener(() =>
        {
            //AudioAssistant.PlaySound("BtnClick");
            AudioManager.instance.Play("BtnClick");
            if (isAnimPlay) return;
            isAnimPlay = true;
            flip = -1;
            MapID--;
        });
        
        btnNext.onClick.RemoveAllListeners();
        btnNext.onClick.AddListener(() =>
        {
            // AudioAssistant.PlaySound("BtnClick");
            AudioManager.instance.Play("BtnClick");
            if (isAnimPlay) return;
            isAnimPlay = true;
            flip = 1;
            MapID++;
        });

        btnCancel.onClick.RemoveAllListeners();
        btnCancel.onClick.AddListener(() => { Close(); });
    }

    private void OnValidate()
    {
        popupID = PopupID.PopupSelectMap;
        if (loadMap)
        {
            loadMap = false;
            mapLevelUIList = GetComponentsInChildren<MapLevelUI>(false).ToList();
            for (int i = 0; mapLevelUIList.Count > i; i++)
            {
                mapLevelUIList[i].mapID = i;
            }
        }
    }

    private int totalLevel;

    public override void OnShow()
    {
        base.OnShow();
        totalLevel = 15;//LevelPrefabManager.Instance.levelPrefabsString.Co
        if (totalLevel > User.Instance[ItemID.PlayingLevel/*PlayingLevelTotalUnlock*/])
        {
            MapID = User.Instance[ItemID.PlayingLevel /*PlayingLevelTotalUnlock*/] / 12;
        }
        else
        {
            MapID = (totalLevel - 1) / 12;
        }

        //textGiftStar.text = $"{User.Instance.TotalStar}/{startneed}";
        //lineStar.fillAmount = (float)User.Instance.TotalStar / (float)startneed;
    }

    public int MapID
    {
        get => mapID;
        set
        {
            if (mapLevelUICurrent == null || value != mapID)
            {
                mapID = value;
                if (mapLevelUICurrent != null)
                {
                    MapLevelUI temp = mapLevelUIList.Where(x => x.mapID == mapID).First();
                    temp.transform.localPosition = new Vector3(flip * widthScreen, 0, 0);
                    temp.TurnOn();
                    temp.transform.DOLocalMoveX(0, 1).SetEase(Ease.OutSine);
                    temp.SetUp();
                    mapLevelUICurrent.transform.DOLocalMoveX(-flip * widthScreen, 1).SetEase(Ease.OutSine).OnComplete(() =>
                    {
                        mapLevelUICurrent.TurnOff();
                        mapLevelUICurrent = temp;
                        isAnimPlay = false;
                    });
                }
                else
                {
                    mapLevelUICurrent = mapLevelUIList.Where(x => x.mapID == mapID).First();
                    mapLevelUICurrent.TurnOn();
                }

                SetStatus();
            }

            mapLevelUICurrent?.SetUp();
        }
    }

    private void SetStatus()
    {
        if (mapID >= (mapLevelUIList.Count - 1))
        {
            btnNext.gameObject.SetActive(false);
        }
        else
        {
            btnNext.gameObject.SetActive(true);
        }

        if (mapID <= 0)
        {
            btnPre.gameObject.SetActive(false);
        }
        else
        {
            btnPre.gameObject.SetActive(true);
        }
    }
}
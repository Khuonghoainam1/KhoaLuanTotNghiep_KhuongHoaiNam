using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using Yurowm.GameCore;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;
    public GameObject hand;
    public const int SortingOrder = 1000;
    public GameObject blockRaycat;
    public TutID menuTut = TutID.None;
    public TutID showingTutID;
    static bool _skipTut = false;


    public static bool skipTut
    {
        get
        {
            return _skipTut;
        }
        set
        {
            _skipTut = value;
            User.Instance.SetCompletedTutID(TutID.Hider);
            User.Instance.SetCompletedTutID(TutID.Seeker);
        }
    }
    //public const int EvovleCompleteLevel = 4;

    public void Init()
    {
        StopAllCoroutines();

        if (skipTut)
        {
            return;
        }

        int level = User.Instance[ItemID.PlayingLevel];

        switch (GameScene.currentSceneName)
        {
            case SceneName.GameScene:
                {
                    if (level == 0 && User.Instance.IsCompletedTutID(TutID.Seeker) == false && User.Instance.SessionCount == 0)
                    {
                        ShowTut(TutID.Seeker);
                    }
                    else if (level == 1 && User.Instance.IsCompletedTutID(TutID.Hider) == false && User.Instance.SessionCount == 0)
                    {
                        ShowTut(TutID.Hider);
                    }
                    break;
                }
            case SceneName.MenuScene:
                {
                    switch (menuTut)
                    {
                        case TutID.None:
                            {
                                //if (User.Instance.IsCompletedTutID(TutID.UpgradeLevel) == false && userHero.CanUpLevel)
                                //{
                                //    ShowTut(TutID.UpgradeLevel);
                                //}

                                break;
                            }
                    }

                    menuTut = TutID.None;

                    break;
                }
        }
    }

    public void ShowTut(TutID id)
    {
        StartCoroutine(IE_ShowTut(id));
    }

    IEnumerator IE_ShowTut(TutID id)
    {
        showingTutID = id;

        switch (id)
        {
            case TutID.Hider:
                {
                    yield return null;
                    break;
                }

            case TutID.Seeker:
                {
                    yield return null;
                    break;
                }
        }

        User.Instance.SetCompletedTutID(id);
    }

    public void OnSetupCamera(Camera camera)
    {
        canvas.worldCamera = camera;
        canvas.planeDistance = 2;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = SortingOrder;
        if (camera.aspect >= 2.1f)
        {
            canvasScaler.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScaler.matchWidthOrHeight = 0;
        }
    }

    bool stepDone = false;
    bool isEndGame = false;
    void OnEndGame()
    {
        isEndGame = true;
    }

    void OnClickTutMove()
    {
        stepDone = true;
    }

    void OnClickTutHide()
    {
        stepDone = true;
    }

    void OnClickTutUI(TutUI tutUI)
    {
        //hand.SetActive(false);
        stepDone = true;
    }

    public void ShowHand(Vector3 pos,bool isShow)
    {
        hand.transform.position = pos;
        hand.SetActive(isShow);
        hand.transform.SetAsLastSibling();
    }

    List<Vector3> listPath;
    bool onPathComplete = false;
}

public enum TutID
{
    None = -1,
    Hider,
    Seeker,
}

public enum TutUI
{
    ButtonMove,
    ButtonHide,
}
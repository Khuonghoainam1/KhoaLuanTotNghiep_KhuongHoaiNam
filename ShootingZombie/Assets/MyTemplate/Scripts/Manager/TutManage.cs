using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using UnityEngine;
using UnityEngine.UI;

public class TutManage : Singleton<TutManage>
{
    public GameObject panelTut;
    //public GameObject TutgamePlay;
    public Button[] ButtonHome;
    //public int index;
    public TutPanel _tutPanel;
    public bool setActitve;
    HomePanelController home;

    public void TutHome(int tut)
    {

        switch (tut)
        {
            //play
            case 0:

                break;

                //Weapon
            case 1:
                if (User.Instance[ItemID.TutUpgradeWeapon] == 0)
                {
                    panelTut.gameObject.SetActive(true);

                }
                else
                {
                    panelTut.gameObject.SetActive(false);
                }
                ButtonHome[tut].onClick.RemoveAllListeners();
                ButtonHome[tut].onClick.AddListener(() =>
                {
                    SetFalse();
                    PopupManager.Instance.OpenPopup<PopupSelectLevel>(PopupID.PopupWorkShop);
                    User.Instance[ItemID.TutUpgradeWeapon] = 1;
                });
                break;

            //upgrade Car
            case 2:
               /* if (User.Instance[ItemID.TutUpgradeCar] == 0)
                {
                    panelTut.gameObject.SetActive(true);

                }
                else
                {
                    panelTut.gameObject.SetActive(false);
                }
                ButtonHome[tut].onClick.RemoveAllListeners();
                ButtonHome[tut].onClick.AddListener(() =>
                {
                    SetFalse();
                    PopupManager.Instance.OpenPopup<PopupSelectLevel>(PopupID.PopupCarUpgrade);
                    User.Instance[ItemID.TutUpgradeCar] = 1;
                });
                break;*/


            //Upgrade Gara
            case 3:
            /*    if (User.Instance[ItemID.TutCollectFuel] == 0)
                {
                    panelTut.gameObject.SetActive(true);
                }
                else
                {
                    panelTut.gameObject.SetActive(false);
                }
                ButtonHome[tut].onClick.RemoveAllListeners();
                ButtonHome[tut].onClick.AddListener(() =>
                {
                    SetFalse();
                    //PopupManager.Instance.OpenPopup<PopupSelectLevel>(PopupID.PopupCarUpgrade);
                    GameScene.main.homePanel.UpdrageHomeStation();
                });
                break;*/

            //mode colect
            case 4:
             /*   if (User.Instance[ItemID.TutCollectFuel] == 0)
                {
                    panelTut.gameObject.SetActive(true);
                }
                else
                {
                    panelTut.gameObject.SetActive(false);
                }
                ButtonHome[tut].onClick.RemoveAllListeners();
                ButtonHome[tut].onClick.AddListener(() =>
                {
                    SetFalse();
                    GlobalData.gameMode = GameMode.CollectFuel;
                    User.Instance.Save();
                    Loader.Instance.LoadScene(SceneName.GameScene.ToString());
                    User.Instance[ItemID.TutCollectFuel] = 1;

                });
                break;*/
            default:
                break;

        }
    }
 
    public void TutWeapons()
    {

    }
    public void Start()
    {
        // index = 0;

        SetTutPanel();
    }
    public void SetTutPanel()
    {
        //index++;
        for (int i = 0; i < ButtonHome.Length; i++)
        {
            if (i == User.Instance.IndexBtnTutData)
            {
                //ButtonHome[i].gameObject.SetActive(true);
                //ButtonHome[i].interactable = true;
                ButtonHome[i].transform.localScale = new Vector3(1, 1, 1);
                TutHome(i);
                Debug.Log(User.Instance.IndexBtnTutData);
            }
            else if (i != User.Instance.IndexBtnTutData)
            {
                ButtonHome[i].transform.localScale = new Vector3(0,0,0);

                //ButtonHome[i].gameObject.SetActive(false);
                //ButtonHome[i].interactable = false;
            }

        }
    }
    /*  void clickButton()
      {
          ButtonHome(index);
      }*/
    void SetFalse()
    {
        panelTut.gameObject.SetActive(false);
    }
}
public enum TutPanel
{
    HomePanel,
    PlayPanel,
}

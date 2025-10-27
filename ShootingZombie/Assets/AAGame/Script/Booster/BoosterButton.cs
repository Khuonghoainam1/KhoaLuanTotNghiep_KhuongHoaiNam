using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BoosterButton : MonoBehaviour
{
    [HideInInspector]
    public Booster booster;
    public Image icon;
    public Image bg;
    public Sprite[] spriteBg;
    public TMP_Text nameBooster;
    private PopupID pop;
    public GameObject tamTut;
    public GameObject TamTutVip;
    public int indexStt = 0;
    [SerializeField] private bool SetCoutDown = true;
    [SerializeField] private Image imgCoutdown;
    [SerializeField] private float TimeCoutdown;
    [SerializeField] private TMP_Text TxtTime;
    [SerializeField] private Button btnInter;
    [SerializeField] private TMP_Text typeBooster;
    public BoosterST BoosterName;
    public GameObject lockImage;
    public GameObject iconADS;

    private void OnEnable()
    {
        GameEvent.OnSetTrueTutVip.RemoveListener(ShowTutVip);
        GameEvent.OnSetTrueTutVip.AddListener(ShowTutVip);


        if(BoosterName == BoosterST.BOOSTER)
        {
            GameEvent.CollectAll.RemoveListener(Select);
            GameEvent.CollectAll.AddListener(Select);
        }


        if(lockImage != null)
        {
            if(User.Instance[ItemID.TutPlay] < 4)
            {
                lockImage.SetActive(true);
                btnInter.interactable = false;
            }
            else
            {
                btnInter.interactable = true;
                lockImage.SetActive(false);
            }
        }




    }



    public void DOAppear()
    {
        this.transform.localScale = Vector3.zero;

        if (BoosterName == BoosterST.BOOSTER)
        {
            this.transform.DOScale(Vector3.one, 1f);
        }
        else if (BoosterName == BoosterST.BOOSTERVIP)
        {
            this.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f);
        }
    }



    public void ShowTutVip(NameBooster nameBooster)
    {
        if (booster.booster == nameBooster)
        {
            TamTutVip.gameObject.SetActive(true);
            btnInter.interactable = true;
            lockImage.SetActive(false);
        }
    }
    public void SetFalseTutVip(NameBooster nameBooster)
    {
        if (booster.booster == nameBooster)
        {
            TamTutVip.gameObject.SetActive(false);
        }
    }
    public void SetUp(Booster booster, PopupID pop,bool isExtra =false)
    {
        this.pop = pop;
        this.booster = booster;
        nameBooster.text = this.booster.nameBooster;   //booster.ToString();
        if(BoosterName== BoosterST.BOOSTER)
        {
            typeBooster.text = this.booster.typeBooster;
        }
        icon.sprite = booster.icon;
        if(bg != null)
        {
            // bg.sprite = spriteBg[Random.Range(0, spriteBg.Length)];
            bg.sprite = booster.bg;
        }
        if (booster.booster == NameBooster.Drone)
        {
            TimeCoutdown = 1f;
        }

        else if (booster.booster == NameBooster.Plane)
        {
            TimeCoutdown = 3f;
        }
        else if (booster.booster == NameBooster.Shield)
        {
            TimeCoutdown = 1f;
        }
        DOAppear();
        //   
        if(BoosterName == BoosterST.BOOSTER)
        {
            if (isExtra)
            {
                //bat
                iconADS.gameObject.SetActive(true);

            }
            else
            {
                //tat
                iconADS.gameObject.SetActive(false);
            }
        }
       
    }

    public void Update()
    {

    }

    public void ShowTut()
    {
        if (User.Instance[ItemID.TutBooster] == 0)
        {
            tamTut.gameObject.SetActive(true);
        }
        else
        {
            tamTut.gameObject.SetActive(false);
        }
    }
    public void Select()
    {
        GameManager.Instance.isSelectBooster = true;
        if (PopupManager.Instance.GetPopup(pop) != null)
        {
            PopupManager.Instance.GetPopup(pop).Close();
        }

        BoosterManager.instance.listBoost.Add(this.booster.booster);
        BoosterManager.instance.boostersSelected.Add(this.booster);
        GameEvent.OnSelectBooster.Invoke();
        if (GameManager.Instance.trainManager.carState == CarState.Stop)
        {
            GameManager.Instance.trainManager.ChangeState(new CarRunningState(GameManager.Instance.trainManager));
        }

        if (booster.type == TypeBooster.Basic)
        {
            GameManager.Instance.isGoingToStation = false;
            GameManager.Instance.isWaitingForUpgrade = false;
        }
        /* if (User.Instance[ItemID.TutBooster] == 0)
         {
             User.Instance[ItemID.TutBooster] = 1;
         }*/
        // AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");

        if (imgCoutdown != null)
        {
            btnInter.interactable = false;
            SetCoutDown = false;
            imgCoutdown.DOFillAmount(1, TimeCoutdown).OnComplete(() =>
            {
                SetCoutDown = true;
                btnInter.interactable = true;
                imgCoutdown.fillAmount = 0;
            });
        }

        Time.timeScale = 1;
    }
    public void SelectBoosterVip()
    {


        GameManager.Instance.isSelectBooster = true;
        if (PopupManager.Instance.GetPopup(pop) != null)
        {
            PopupManager.Instance.GetPopup(pop).Close();
        }

        BoosterManager.instance.listBoost.Add(this.booster.booster);
        BoosterManager.instance.boostersSelected.Add(this.booster);
        GameEvent.OnSelectBooster.Invoke();
        if (GameManager.Instance.trainManager.carState == CarState.Stop)
        {
            GameManager.Instance.trainManager.ChangeState(new CarRunningState(GameManager.Instance.trainManager));
        }

        if (booster.type == TypeBooster.Basic)
        {
            GameManager.Instance.isGoingToStation = false;
            GameManager.Instance.isWaitingForUpgrade = false;
        }
        /* if (User.Instance[ItemID.TutBooster] == 0)
         {
             User.Instance[ItemID.TutBooster] = 1;
         }*/
        // AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");

        if(User.Instance[ItemID.TutPlay] < 4)
        {
            lockImage.SetActive(true);
            btnInter.interactable = false;
        }
        else
        {
            if (imgCoutdown != null)
            {
                btnInter.interactable = false;
                SetCoutDown = false;
                imgCoutdown.DOFillAmount(1, TimeCoutdown).OnComplete(() =>
                {
                    SetCoutDown = true;
                    btnInter.interactable = true;
                    imgCoutdown.fillAmount = 0;
                });
            }
        }
     
            if (User.Instance[ItemID.TutPlay] == 1)
            {
                SetFalseTutVip(NameBooster.Drone);
                User.Instance[ItemID.TutPlay] = 4;
            }
            else if (User.Instance[ItemID.TutPlay] == 2)
            {
                SetFalseTutVip(NameBooster.Plane);
                User.Instance[ItemID.TutPlay] = 3;
            }
            else if (User.Instance[ItemID.TutPlay] == 3)
            {
                SetFalseTutVip(NameBooster.Shield);
                User.Instance[ItemID.TutPlay] = 4;
            }

        Time.timeScale = 1;
    }
    public void CollectAllBooster()
    {
        Select();
    }
}
public enum BoosterST
{
    BOOSTERVIP,
    BOOSTER,
}
using UnityEngine;
using Spine.Unity;

public class CarUI : MonoBehaviour
{
    public UserCar userCar;
    public SkeletonGraphic skeletonGraphic;
    public BotUI player;
    public BotUI bot1;
    public BotUI bot2;
    public BotUI bot3;
    public GameObject tutSelectBot1;
    public GunSkinControl gunSkinControl;

    public void OnEnable()
    {
        SetUp();
        GameEvent.OnCarLevelUp.RemoveListener(SetUp);
        GameEvent.OnCarLevelUp.AddListener(SetUp);
    }

    private void Start()
    {
        
    }

    public void SetUp()
    {
        userCar = User.Instance.Car;

        //thay skin xe 
        skeletonGraphic.Skeleton.SetSkin(userCar.skin + "/phase_1");
        skeletonGraphic.Skeleton.SetSlotsToSetupPose();
        //add nut select bot neu co slot da mo
    }

    public void SpawnBot()
    {
        gunSkinControl.gameObject.SetActive(false);
        player.gameObject.SetActive(true);
        player.SetUp(User.Instance.UserPlayerUsing, 0);

        //spawn bot
        if (User.Instance.Car.slot == 1)
        {
            bot1.gameObject.SetActive(true);
            bot1.SetUp(User.Instance.UserBot1Using, 1);
        }
        if (User.Instance.Car.slot == 2)
        {
            bot1.gameObject.SetActive(true);
            bot1.SetUp(User.Instance.UserBot1Using, 1);

            bot2.gameObject.SetActive(true);
            bot2.SetUp(User.Instance.UserBot2Using, 2);
        }
        if (User.Instance.Car.slot == 3)
        {
            bot1.gameObject.SetActive(true);
            bot1.SetUp(User.Instance.UserBot1Using, 1);

            bot2.gameObject.SetActive(true);
            bot2.SetUp(User.Instance.UserBot2Using, 2);

            bot3.gameObject.SetActive(true);
            bot3.SetUp(User.Instance.UserBot3Using, 3);
        }

        //OFF TUT
        if (User.Instance[ItemID.TutUpgradeCar] < 2)
        {
            tutSelectBot1.SetActive(true);
        }
        else
        {
            tutSelectBot1.SetActive(false);
        }
    }

    public void HideBot()
    {
        gunSkinControl.gameObject.SetActive(true);
        player.gameObject.SetActive(false);
        bot1.gameObject.SetActive(false);
        bot2.gameObject.SetActive(false);
        bot3.gameObject.SetActive(false);
    }
}

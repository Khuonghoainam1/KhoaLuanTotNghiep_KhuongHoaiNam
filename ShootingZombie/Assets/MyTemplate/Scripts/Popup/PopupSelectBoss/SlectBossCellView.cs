using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Thanh.Core;
using Spine.Unity;

public class SlectBossCellView : MonoBehaviour
{
    [SerializeField]
    private Image bg;
    [SerializeField]
    private Image imgBgLook;
    [SerializeField]
    private TMP_Text Name;
    [SerializeField]
    private LevelBossSetUp LevelBos;
    [SerializeField]
    private Button clickBoss;
    [SerializeField]
    private Button clickReview;
    public int level;
    public SkeletonGraphic SkeletonGraphic;/*{ get; private set; }*/


    public void SetData(LevelBossSetUp levelBossSetUp)
    {
        LevelBos = levelBossSetUp;
        levelBossSetUp.ChangeSkeletonDataAsset(SkeletonGraphic);
     
        Name.text = LevelBos.Name.ToString() ;
        bg.sprite = LevelBos.avt;
        clickBoss.onClick.RemoveListener(OnClickBoss);
        clickBoss.onClick.AddListener(OnClickBoss);
        clickReview.onClick.RemoveListener(OnClickReview);
        clickReview.onClick.AddListener(OnClickReview);

    }
    
    public void OnClickBoss()
    {
        
        GlobalData.instance.levelToPlay = this.level;
        GlobalData.instance.bossToFight = LevelBos.bossId;
        GameEvent.OnMoveToPlay.Invoke();
        /* PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) => {
         pop.Items[0].gameObject.SetActive(true);
         pop.Items[1].gameObject.SetActive(true);
         pop.Items[2].gameObject.SetActive(false);
         pop.index = 0;

             pop.SetData(0);
         });*/
    }
    public void OnClickReview()
    {


        GlobalData.instance.levelToPlay = this.level;
        GlobalData.instance.bossToFight = LevelBos.bossId;
        PopupManager.Instance.OpenPopup<PopupReviewReward>(PopupID.PopupReviewReward, (pop) => {
            pop.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            pop.Items[0].gameObject.SetActive(true);
            pop.Items[1].gameObject.SetActive(false);
            pop.Items[2].gameObject.SetActive(true);
            pop.index = 0;
            pop.ClickPlay.gameObject.SetActive(false);
            pop.txtReview.gameObject.SetActive(false);
            pop.txtboss.gameObject.SetActive(true);
            pop.txtGold.gameObject.SetActive(true);
            pop.txtTiket.gameObject.SetActive(true);
            pop.txtGold.text ="+" + GameManager.Instance.rewardBoss.rewardConfig[level].gold.ToString();
            pop.txtTiket.text = "+" + GameManager.Instance.rewardBoss.rewardConfig[level].ticket.ToString();
            LevelBos.ChangeSkeletonDataAsset(pop.SkinIcon);
            pop.SetData(0);
        });
    }
}

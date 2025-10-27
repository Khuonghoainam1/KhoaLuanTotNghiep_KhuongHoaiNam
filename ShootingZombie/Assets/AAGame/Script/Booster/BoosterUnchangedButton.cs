using UnityEngine;
using UnityEngine.UI;

public class BoosterUnchangedButton : MonoBehaviour
{
    public GameObject tickIcon;
    public Booster booster;
    public Button viewAdsBtn;
    [SerializeField] private Image bg;
    [SerializeField] private Image imgIcon;

    [SerializeField] private Sprite bgBoosterTrue;
    [SerializeField] private Sprite bgBoosterFalse;
    [SerializeField] private Sprite sprIconFalse;
    [SerializeField] private Sprite sprIconTrue;
    private void OnEnable()
    {
        viewAdsBtn.onClick.RemoveListener(ViewAdsGetBooster);
        viewAdsBtn.onClick.AddListener(ViewAdsGetBooster);
        if (BoosterManager.instance.listBoost.Contains(this.booster.booster))
        {
            viewAdsBtn.gameObject.SetActive(false);
            bg.sprite = bgBoosterTrue;
            imgIcon.sprite = sprIconTrue;
            viewAdsBtn.gameObject.SetActive(false);
        }
        else
        {
            viewAdsBtn.gameObject.SetActive(true);
            bg.sprite = bgBoosterFalse;
            imgIcon.sprite = sprIconFalse;
        }
    }

    public void ViewAdsGetBooster()
    {
        BoosterManager.instance.listBoost.Add(this.booster.booster);
        bg.sprite = bgBoosterTrue;
        imgIcon.sprite = sprIconTrue;
        viewAdsBtn.gameObject.SetActive(false);
    }
}

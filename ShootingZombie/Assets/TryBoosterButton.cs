using DG.Tweening;
using UnityEngine;

public class TryBoosterButton : MonoBehaviour
{
    public Booster booster;
    public PopupTryHero popupTry;

    private void OnEnable()
    {
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(new Vector3(1.4f,1.4f,1), 0.5f);
    }


    public void Select()
    {
        GameManager.Instance.isSelectTryHero = true;
        BoosterManager.instance.listBoost.Add(this.booster.booster);
        BoosterManager.instance.boostersSelected.Add(this.booster);
        GameEvent.OnSelectBooster.Invoke();
        popupTry.Close();
    }
}

using DG.Tweening;
using Thanh.Core;
using TMPro;

public class PopupStage : Popup
{
    public TMP_Text txtStageLevel;

    public override void OnShow()
    {
        base.OnShow();
        gameObject.transform.DOLocalMoveX(0, 0.6f).From(3000).OnComplete(() =>
        {
            gameObject.transform.DOLocalMoveX(-3000, 0.6f).From(0).SetDelay(0.3f).OnComplete( () => { this.Close(); });
        });
        txtStageLevel.text = "STAGE " + (GlobalData.instance.levelToPlay + 1).ToString();

    }
}

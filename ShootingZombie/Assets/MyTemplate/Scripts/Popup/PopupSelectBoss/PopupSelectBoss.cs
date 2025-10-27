using Thanh.Core;

public class PopupSelectBoss : Popup
{
    private void OnEnable()
    {
        GameEvent.OnMoveToPlay.RemoveListener(OnMoveToPlay);
        GameEvent.OnMoveToPlay.AddListener(OnMoveToPlay);
    }

    public void OnMoveToPlay()
    {
        this.Close();

      
    }
}

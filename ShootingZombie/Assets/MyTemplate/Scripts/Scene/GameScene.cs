using Thanh.Core;

public class GameScene : BaseScene
{
    public BoosterPanel boosterPanel;
    public InventoryPanel inventoryPanel;
    public PlayingPanelController popupPlaying;
    public PausePanelController popupPause;
    public HomePanelController homePanel;

    public static GameScene _main;
    public static GameScene main
    {
        get
        {
            if (_main == null)
            {
                _main = Instance as GameScene;
            }

            return _main;
        }
    }

    public RotateAndDragOnMouseClick checker;
    
    public override void Init()
    {

    }
}

using UnityEngine;
using UnityEngine.UI;

public class CellTageLevel : MonoBehaviour
{
    public Sprite spriteLevelDone;// Level Da xong
    public Sprite spriteLevelStart;// Level dang choi
    public Sprite spriteLevelNext;//Level tiep theo
    public Image imgStageLevel;
    public int stage;
    public Image imgIconBoss;
    public void SetUp(int stage)
    {
        this.stage = stage;
        if(stage < User.Instance[ItemID.PlayingLevel]+1)
        {
            imgStageLevel.sprite = spriteLevelDone;
        }
        else if(stage == User.Instance[ItemID.PlayingLevel]+1)
        {
            imgStageLevel.sprite = spriteLevelStart;
        }
        else
        {
            imgStageLevel.sprite = spriteLevelNext;
        }

        if(this.stage != 0 && this.stage % 6 == 0)
        {
            imgIconBoss.gameObject.SetActive(true);
        }
        else
        {
            imgIconBoss.gameObject.SetActive(false);
        }
    }
}

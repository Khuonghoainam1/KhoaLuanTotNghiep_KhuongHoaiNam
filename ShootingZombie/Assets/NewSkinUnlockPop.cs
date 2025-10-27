using UnityEngine;
using UnityEngine.UI;

public class NewSkinUnlockPop : MonoBehaviour
{
    public Image icon;
    public GameObject slotUp;
    private UserBot userBot;

    public void SetUp(UserBot userBot,bool isSlot = false)
    {
        this.userBot = userBot;
        icon.sprite = Resources.Load<Sprite>("IconBot/" + userBot.id.ToString());
        if (isSlot)
        {
            slotUp.SetActive(true);
        }
        else
        {
            slotUp.SetActive(false);
        }
        icon.SetNativeSize();
    }

    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}

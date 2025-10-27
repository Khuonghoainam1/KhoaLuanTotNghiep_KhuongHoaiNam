using UnityEngine;
using UnityEngine.UI;

public class IconHero : MonoBehaviour
{
    public Image icon;
    public TypeBot typeBot;

    private void Start()
    {
        GameEvent.OnEquipSkin.AddListener(SetIcon);
        SetIcon(null);
    }

    public void SetIcon(UserBot temp)
    {
        switch (typeBot)
        {
            case TypeBot.Player:
                icon.sprite = Resources.Load<Sprite>("IconBot/" + User.Instance.UserPlayerUsing.id.ToString());
                break;
            case TypeBot.Pistol:
                icon.sprite = Resources.Load<Sprite>("IconBot/" + User.Instance.UserBot1Using.id.ToString());
                break;
            case TypeBot.Riffle:
                icon.sprite = Resources.Load<Sprite>("IconBot/" + User.Instance.UserBot2Using.id.ToString());
                break;
            case TypeBot.Bazoka:
                icon.sprite = Resources.Load<Sprite>("IconBot/" + User.Instance.UserBot3Using.id.ToString());
                break;
        }
    }
}

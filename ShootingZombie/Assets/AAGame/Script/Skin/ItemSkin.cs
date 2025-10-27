using AA_Game;
using UnityEngine.UI;
using UnityEngine;

public class ItemSkin : Item
{
    public Image BG;
    public Sprite[] bgs;  //0- normal , 1-select , 2-dang su dung
    public Image icon;
    public Image block;

    public int indexBot;
    public UserBot bot;
    public UserBot botUsing;

    public Image imageSelect;
    public GameObject[] stars;


    private void OnEnable()
    {
        GameEvent.OnSelectSkin.RemoveListener(OnOtherSkinSlect);
        GameEvent.OnSelectSkin.AddListener(OnOtherSkinSlect);
    }

    public void SetStar()
    {
        foreach(GameObject star in stars)
        {
            star.gameObject.SetActive(false);
            for(int i = 0; i < bot.star; i++)
            {
                stars[i].SetActive(true);
            }
        }
    }

    public void SetUp(UserBot bot)
    {
        this.bot = bot;
        icon.sprite = Resources.Load<Sprite>("IconBot/NewIcon/" + bot.id.ToString());

        //kiem tra skin da duoc mo khoa hay chua
        if (bot.isUnlock)
        {
            block.gameObject.SetActive(false);
        }
        else
        {
            block.gameObject.SetActive(true);
        }

        //kiem tra co phai skin dang su dung hay ko
        if (bot.isUsing)
        {
            BG.sprite = bgs[2];
        }
        SetStar();
    }

    public void Select()
    {
        GameEvent.OnSelectSkin.Invoke(this.bot);
        if (bot.isUsing)
        {
            BG.sprite = bgs[2];
            imageSelect.gameObject.SetActive(true);
        }
        else
        {
            BG.sprite = bgs[0];
            imageSelect.gameObject.SetActive(true);
        }
    }

    public void OnOtherSkinSlect(UserBot userBot)
    {
        imageSelect.gameObject.SetActive(false);
        BG.sprite = bgs[0];
        if (bot.isUnlock)
        {
            block.gameObject.SetActive(false);
        }
        else
        {
            block.gameObject.SetActive(true);
        }
        if (bot.isUsing)
        {
            BG.sprite = bgs[2];
        }

        if (this.bot.id == userBot.id)
        {
            imageSelect.gameObject.SetActive(true);
            SetStar();
        }
    }

    public void UnLock()
    {
        switch (indexBot)
        {
            case 0:
                //player
                break;
            case 1:
                //bot 1
                break;
            case 2:
                //bot2
                break;
            case 3:
                //bot3
                break;
        }
    }
}

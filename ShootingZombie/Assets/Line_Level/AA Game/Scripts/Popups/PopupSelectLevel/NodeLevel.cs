using AA_Game;
using System.Collections;
using System.Collections.Generic;
using Thanh.Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Yurowm.GameCore;

public class NodeLevel : MonoBehaviour
{
    [SerializeField] private Image imageBgText;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Image imgNode;
    [SerializeField] private List<Image> imagesStarNomal;
    [SerializeField] private Sprite imgStarTrue;
    [SerializeField] private Button btnClick;
    [SerializeField] private Sprite nodeOn, nodeOff;
    private Item tam;
    public int level;

    public void SetupNode(int level)
    {
        this.level = level;
        if (tam != null)
        {
            tam.Kill();
            tam = null;
        }
        txtLevel.text = (level + 1) % 6 == 0
            ? (level / 6) % 2 == 0 ? "Mini Boss" : "Boss"
            : $"{(level + 1) - ((level + 1) / 6)}";
        imageBgText.gameObject.SetActive(!((level + 1) % 6 == 0));
        //imgNode.sprite = //SpriteManager.Instance.GetSprite(ItemID.IconNodeMapOff);
        // imageBgText.sprite = SpriteManager.Instance.GetSprite(ItemID.IconBGTextOff);
        if (User.Instance[ItemID.PlayingLevel] >= level )
        {
            imgNode.sprite = nodeOn;
             btnClick.onClick.RemoveAllListeners();
            btnClick.onClick.AddListener(OnClickNode);
        }
        else
        {
            btnClick.onClick.RemoveAllListeners();
            imgNode.sprite = nodeOff;
        }
        //tam = ContentPoolable.Emit(ItemID.TamUI);
        //tam.transform.SetParent(transform, false);

        //imgNode.sprite = SpriteManager.Instance.GetSprite(ItemID.IconNodeMapOn);
        // unlock


        if(this.level <= User.Instance[ItemID.PlayingLevel])
        {
            RandomSetTrueStar();
        }
    }

    public void RandomSetTrueStar()
    {
        // int randomstar = Random.Range(0, 4);
        //for(int i = 0; i < randomstar; i++)
        //{
        //    imagesStarNomal[i].sprite = imgStarTrue;
        //}         

        for (int i = 0; i < User.Instance.ListStarLevel()[this.level].starAmount; i++)
        {
            imagesStarNomal[i].sprite = imgStarTrue;
        }
    }

    private void OnValidate()
    {
        txtLevel = GetComponentInChildren<TextMeshProUGUI>(false);
        btnClick = GetComponent<Button>();
        imgNode = GetComponent<Image>();
        //imageBgText.sprite = SpriteManager.Instance.GetSprite(ItemID.IconBGTextOff);

        //Set image default
        //imagesStarNomal.ForEach(img => { img.sprite = SpriteManager.Instance.GetSprite(ItemID.IconStarOff); });
        //imgNode.sprite = SpriteManager.Instance.GetSprite(ItemID.IconNodeMapOff);
    }

    private void OnClickNode()
    {
        // Click Node
      //  AudioAssistant.PlaySound("BtnClick");
        AudioManager.instance.Play("BtnClick");

         GlobalData.instance.levelToPlay = this.level;      
         User.Instance[ItemID.IsAutoPlay] = 1;
       // User.Instance[ItemID.TutPlay] = 1;
         Loader.Instance.LoadScene(SceneName.GameScene.ToString());
        //RandomSetTrueStar();

        //if (data == null)
        //{
        //    NotiTextUI notiTextUI = ContentPoolable.Emit(ItemID.NotiTextUI) as NotiTextUI;
        //    notiTextUI.transform.SetParent(MenuScene.main.transform, false);
        //    notiTextUI.SetUp("Level Non Unlock");
        //}
        //else
        //{

        //PopupSelectLevel popupSelectLevel =
        //    PopupManager.Instance.GetPopup(PopupID.PopupSelectMap) as PopupSelectLevel;
        //PopupManager.Instance.OpenPopup<PopupInfomap>(PopupID.PopupInfoMap,
        //    (pop) => { pop.Setup(popupSelectLevel.MapID, data); });
        //}
    }
}
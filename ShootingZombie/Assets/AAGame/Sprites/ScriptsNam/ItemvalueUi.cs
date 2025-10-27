using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemvalueUi : MonoBehaviour
{
    public ItemID itemID;
    public Image icon;
    public TextMeshProUGUI txtValue;

    private void OnEnable()
    {
        if (icon != null)
        {
            icon.sprite = SpriteManager.Instance.GetSprite(itemID);
        }
        Refresh();

        //gameObject.SetActive(BaseScene.currentSceneName != SceneName.GameScene);

        GameEvent.OnItemChanged.RemoveListener(OnItemChanged);
        GameEvent.OnItemChanged.AddListener(OnItemChanged);
    }

    private void Start()
    {
        
    }

    void OnItemChanged(ItemID id, int value)
    {
        if (id != itemID)
        {
            return;
        }

        Refresh();
    }

    void Refresh()
    {
        txtValue.text = User.Instance[itemID].ToKMB();
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemDropUI : MonoBehaviour
{
    public GameObject bg1;
    public GameObject bg2;
    public Image iconItem;
    public TMP_Text amountItem;
    private int amount;
    public bool isCoin;


    public void SetUp(string icon,int amount,bool isCoin=false)
    {
        this.isCoin = isCoin;
        this.amount = amount;
        if (Random.Range(0, 2) == 1)
        {
            bg1.SetActive(true);
        }
        else
        {
            bg2.SetActive(true);
        }

        iconItem.sprite = Resources.Load<Sprite>("Sprite/ItemDrop/" + icon);
        amountItem.text = amount.ToString();
    }
    private void Update()
    {
        if (isCoin)
        {
            amountItem.text = (amount * GameManager.Instance.numberMul).ToString();
        }
    }
}

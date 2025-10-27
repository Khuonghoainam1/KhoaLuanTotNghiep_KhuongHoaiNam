using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PieceGun : MonoBehaviour
{
    public ItemID IDManhSung;
    public TMP_Text amountText;
    private int amount;
    public Image notEnought;

    private void OnEnable()
    {
        amount = User.Instance[IDManhSung];
        amountText.text = User.Instance[IDManhSung].ToString();
    }

    public void SetUp(int amountNeed)
    {
        if (amountNeed <= amount)
        {
            notEnought.gameObject.SetActive(false);
        }
        else
        {
            notEnought.gameObject.SetActive(true);
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    public List<GunButton> gunSlots = new List<GunButton>();
    public Gun buttonSellect;
    public Gun buttonTemp;


    private void Start()
    {
       // OnShow();
    }

    public void OnShow()
    {
        //Debug.Log("inventory count--" + User.Instance.ListInventory().Count);
        buttonSellect = null;
        for (int i = 0; i < 25; i++)
        {
            //gunSlots[i].SetUp(User.Instance.ListInventory()[i]);
        }
    }

    public void SaveInventory()
    {
        for (int i = 0; i < 25; i++)
        {
           // User.Instance.ListInventory()[i] = gunSlots[i].gun;
            User.Instance.Save();
        }
    }
}

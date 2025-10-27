using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLeveControler : MonoBehaviour
{
    public List<CellTageLevel> cells = new List<CellTageLevel>();

    public void OnEnable()
    {
        SetUp();
    }

    public void SetUp()
    {
        if ((int)(User.Instance[ItemID.PlayingLevel] / 6) == 0)
        {
            for (int i = 1; i <= 6; i++)
            {
                cells[i - 1].SetUp(i);
            }
        }
        else
        {
            int levelDau = (6 * ((int)(User.Instance[ItemID.PlayingLevel] / 6)) +1);
            int levelCuoi = levelDau + 5;
            for(int i = levelDau; i <=levelCuoi; i++)
            {
                cells[i - levelDau].SetUp(i);
            }
        }
    }
}

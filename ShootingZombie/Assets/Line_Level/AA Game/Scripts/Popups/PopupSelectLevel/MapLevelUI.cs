using AA_Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Yurowm.GameCore;

public class MapLevelUI : Item
{
    public int mapID;
    [SerializeField] private int fromLevel;
    [SerializeField] private TextMeshProUGUI txtChaper;
    [SerializeField] private NodeLevel[] levels;
    [SerializeField] private UILineRenderer _lineRendererOff;
    [SerializeField] private UILineRenderer _lineRendererOn;

    private void OnValidate()
    {
        //levels = GetComponentsInChildren<NodeLevel>();
        fromLevel = mapID * 12;
        //_lineRendererOff = GetComponentInChildren<UILineRenderer>();
        //for (int i = 0; i < levels.Length; i++)
        //{
        //    _lineRendererOff.Points[i] = levels[i].transform.rect().anchoredPosition;
        //}
    }

    public void SetUp()
    {
        txtChaper.text = "Chapter " + (mapID + 1);
        for (int i = fromLevel; i < fromLevel + 12; i++)
        {
            levels[i % 12].SetupNode(i);
        }

        int levelUnlock = User.Instance[ItemID.PlayingLevel/*PlayingLevelTotalUnlock*/];
        int indexPosUnlock = (levelUnlock - fromLevel) % 12;
        _lineRendererOff.Clear();
        _lineRendererOn.Clear();

        if (levelUnlock + 1 > fromLevel + 12)
        {
            _lineRendererOn.Points = new Vector2[levels.Length];
            for (int i = 0; i < levels.Length; i++)
            {
                _lineRendererOn.Points[i] = levels[i].transform.rect().anchoredPosition;
            }
        }
        else
        {
            if (indexPosUnlock > 0)
            {
                _lineRendererOn.Points = new Vector2[indexPosUnlock + 1];
                _lineRendererOff.Points = new Vector2[levels.Length - indexPosUnlock];
                for (int i = 0; i <= indexPosUnlock; i++)
                {
                    _lineRendererOn.Points[i] = levels[i].transform.rect().anchoredPosition;
                }

                for (int i = indexPosUnlock; i < levels.Length; i++)
                {
                    _lineRendererOff.Points[i - indexPosUnlock] = levels[i].transform.rect().anchoredPosition;
                }
            }
            else
            {
                _lineRendererOff.Points = new Vector2[levels.Length];
                for (int i = 0; i < levels.Length; i++)
                {
                    _lineRendererOff.Points[i] = levels[i].transform.rect().anchoredPosition;
                }
            }
        }
    }

    public void TurnOn()
    {
        gameObject.SetActive(true);
    }

    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
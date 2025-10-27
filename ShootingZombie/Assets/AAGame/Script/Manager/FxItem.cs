using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using AA_Game;

public class FxItem : Item
{
    public SortingGroup sortingGroup;
    public List<AudioClip> clips;

    private void OnValidate()
    {
        if (sortingGroup == null)
        {
            sortingGroup = GetComponent<SortingGroup>();
        }

        if (clips != null && clips.Count > 0)
        {
            clips.RemoveAll(x => x == null);
        }
    }

    public override void Initialize()
    {
        base.Initialize();
        if (clips != null && clips.Count > 0)
        {
            AudioAssistant.PlaySound(clips.GetRandom());
        }
    }
}

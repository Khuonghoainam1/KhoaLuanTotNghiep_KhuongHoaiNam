using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class MyTag
{
    private static Dictionary<TagID, string> tagDic = new Dictionary<TagID, string>
    {
        { TagID.Player, "Player" },
        { TagID.Enemy, "Enemy" },
        { TagID.Food, "Food" },
        { TagID.Bullet, "Bullet" },
        { TagID.Default, "Default" },
        { TagID.Untagged, "Untagged" },
        { TagID.Pet, "Pet" }
    };

    public static string Get(TagID tagID)
    {
        return tagDic[tagID];
    }
}

public enum TagID
{
    Player,
    Enemy,
    Food,
    Bullet,
    Default,
    Untagged,
    Pet
}

public static class MySortingLayer
{
    public const int CharacterBackOrder = 0;
    public const int CharacterNormalOrder = 10;
    public const int CharacterAttackOrder = 20;
    public const int SkillFrontPlayerOrder = 30;
    public const int ForegroundEffectOrder = 100;

    private static Dictionary<SortingLayerID, string> layerDic = new Dictionary<SortingLayerID, string>
    {
        { SortingLayerID.Slot, "Slot" },
        { SortingLayerID.Character, "Character" },
        { SortingLayerID.UI, "UI" },        
        { SortingLayerID.Foreground, "Foreground" },
    };

    public static string Get(SortingLayerID id)
    {
        return layerDic[id];
    }

    public static void SetSortingLayer(this Renderer r, SortingLayerID layerID, int order = 0)
    {
        r.sortingLayerID = SortingLayer.NameToID(Get(layerID));
        r.sortingOrder = order;
    }

    public static void SetSortingLayer(this SortingGroup group, SortingLayerID layerID, int order = 0)
    {
        group.sortingLayerID = SortingLayer.NameToID(Get(layerID));
        group.sortingOrder = order;
    }
}

public enum SortingLayerID
{
    Slot,
    Character,
    UI,    
    Foreground,
    BackGround,
    Defaut,
}

public static class MyLayer
{
    private static Dictionary<LayerID, string> layerDic = new Dictionary<LayerID, string>
    {
        { LayerID.Slot, "Slot" },
        { LayerID.Character, "Character" },
    };

    public static string Get(LayerID id)
    {
        return layerDic[id];
    }

    public static void SetLayerRecursive(this GameObject o, LayerID layer)
    {
        o.SetLayerRecursive(LayerMask.NameToLayer(Get(layer)));
    }

    public static void SetLayerRecursive(this GameObject o, int layer)
    {
        SetLayerInternal(o.transform, layer);
    }

    private static void SetLayerInternal(Transform t, int layer)
    {
        t.gameObject.layer = layer;

        foreach (Transform o in t)
        {
            SetLayerInternal(o, layer);
        }
    }
}

public enum LayerID
{
    Slot,
    Character
}
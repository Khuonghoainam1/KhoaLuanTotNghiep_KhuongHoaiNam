using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : MonoBehaviour
{
    public List<Booster> boosters = new List<Booster>();
    public List<Booster> boostersSelected = new List<Booster>();
    public List<Booster> boosterVip = new List<Booster>();
    public List<NameBooster> listBoost = new List<NameBooster>();
    public List<Booster> temp = new List<Booster>();
    public List<Booster> boostersExtra = new List<Booster>();
    public List<Booster> tryBoosters = new List<Booster>();

    public static BoosterManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //temp = new List<Booster>();
    }
    public List<Booster> GetListBoosterRandom()
    {
        List<Booster> boosters = new List<Booster>();
        temp.Clear();
        temp.AddRange(this.boosters);
        if (boostersSelected.Count > 0)
        {
            foreach(Booster boost in boostersSelected)
            {
                if(boost.booster != NameBooster.PercentHealth20 && boost.booster != NameBooster.PercentHealth50 && boost.booster != NameBooster.PercentDamage10)
                {
                    temp.Remove(boost);
                }
            }
        }        
        for (int i = 0; i < 2; i++)
        {
            Booster booster = temp.GetRandom();
            temp.Remove(booster);
            boosters.Add(booster);
        }
        
        return boosters;
    }


    public Booster GetBoosterExtraRandom()
    {
        return boostersExtra.GetRandom();
    }
}

using Lean.Pool;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Yurowm.GameCore;

public class ILiveContentPoolable : ILiveContent, IPoolable
{
    public bool isQuery = false;
    public List<QueryID> queryIndexs = new List<QueryID>();

    public bool autoKill = false;
    [Sirenix.OdinInspector.ShowIf("autoKill")]
    public float liveTime = 0f;

    static List<ILiveContentPoolable>[] queryContent;
    static List<UnityEvent> OnContentChanged = new List<UnityEvent>();

    public UnityEvent OnSpawnEvent = new UnityEvent();
    public UnityEvent OnDespawnEvent = new UnityEvent();

    public static void InitAll()
    {
        queryContent = new List<ILiveContentPoolable>[(int)QueryID.Max];
        OnContentChanged = new List<UnityEvent>();
        for (int i = 0; i < queryContent.Length; i++)
        {
            queryContent[i] = new List<ILiveContentPoolable>();
            OnContentChanged.Add(new UnityEvent());
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        if (isQuery)
        {
            for (int i = 0; i < queryIndexs.Count; i++)
            {
                queryContent[(int)queryIndexs[i]].Add(this);
                OnContentChanged[(int)queryIndexs[i]]?.Invoke();
            }
        }

        if (autoKill && liveTime > 0)
        {
            StartCoroutine(IE_WaitDestroy());
        }

        if (OnSpawnEvent != null)
        {
            OnSpawnEvent?.Invoke();
            OnSpawnEvent.RemoveAllListeners();
        }
    }

    //giang 26/7
    void OnEnable()
    {
        if (autoKill && liveTime > 0)
        {
            StartCoroutine(IE_WaitDestroy());
        }
    }

    private void BaseKill()
    {
        if (isQuery)
        {
            for (int i = 0; i < queryIndexs.Count; i++)
            {
                queryContent[(int)queryIndexs[i]].Remove(this);
                OnContentChanged[(int)queryIndexs[i]]?.Invoke();
            }
        }
        ILiveContent.all.Remove(this);
        this.OnKill();
        if (OnDespawnEvent != null)
        {
            OnDespawnEvent?.Invoke();
            OnDespawnEvent.RemoveAllListeners();
        }
    }

    public override void Kill()
    {
        BaseKill();
        LeanPool.Despawn(this);
    }

    public void WaitToKill(float liveTime)
    {
        StartCoroutine(IE_WaitToKill(liveTime));
    }

    IEnumerator IE_WaitToKill(float liveTime)
    {
        yield return WaitForSecondsCache.Get(liveTime);
        Kill();
    }

    public void DestroyObj()
    {
        BaseKill();
        Destroy(gameObject);
    }

    public virtual void OnSpawn()
    {
        if (autoKill && liveTime > 0)
        {
            StartCoroutine(IE_WaitDestroy());
        }
    }

    IEnumerator IE_WaitDestroy()
    {
        yield return WaitForSecondsCache.Get(liveTime);

        OnAutoDestroy();
    }

    public virtual void OnAutoDestroy()
    {
        Kill();
    }

    public virtual void OnDespawn()
    {

    }

    public new static void KillEverything()
    {
        OnContentChanged.ForEach(x => x.RemoveAllListeners());
        ILiveContent.KillEverything();
        OnContentChanged.Clear();
    }

    public static IEnumerable<T> GetAll<T>(QueryID id) where T : ILiveContentPoolable
    {
        return queryContent[(int)id].Select(x => x as T).Where(x => x != null);
    }

    public static IEnumerable<T> GetAll<T>(List<QueryID> ids) where T : ILiveContentPoolable
    {
        return ids.SelectMany(id => queryContent[(int)id].Select(x => x as T)).Where(x => x != null);
    }

    public static void AddListener(QueryID queryID, UnityAction action)
    {
        OnContentChanged[(int)queryID].AddListener(action);
    }

    public static void RemoveListener(QueryID queryID, UnityAction action)
    {
        OnContentChanged[(int)queryID].RemoveListener(action);
    }
}

public enum QueryID
{
    None = -1,
    AllEnemy = 0,
    Enemy = 1,
    Player = 2,
    AllCharacter = 3,
    Max,
}
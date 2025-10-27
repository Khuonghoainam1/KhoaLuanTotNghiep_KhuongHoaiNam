using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaitForSecondsCache
{
    public readonly static WaitForSeconds WAIT_ONE = new WaitForSeconds(1f);
    public readonly static WaitForSeconds WAIT_TWO = new WaitForSeconds(2f);

    public readonly static WaitForSecondsRealtime WAIT_REAL_ONE = new WaitForSecondsRealtime(1f);
    public readonly static WaitForSecondsRealtime WAIT_REAL_TWO = new WaitForSecondsRealtime(2f);

    private static Dictionary<float, WaitForSeconds> _dictionaryWPS = new Dictionary<float, WaitForSeconds>();
    private static Dictionary<float, WaitForSecondsRealtime> _rdictionaryWPS = new Dictionary<float, WaitForSecondsRealtime>();

    public static WaitForSeconds Get(float key)
    {
        if (!_dictionaryWPS.ContainsKey(key))
        {
            _dictionaryWPS[key] = new WaitForSeconds(key);
        }

        return _dictionaryWPS[key];
    }

    public static WaitForSecondsRealtime GetRealTime(float key)
    {
        if (!_rdictionaryWPS.ContainsKey(key))
        {
            _rdictionaryWPS[key] = new WaitForSecondsRealtime(key);
        }

        return _rdictionaryWPS[key];
    }
}

public class MyWaitForSeconds : IEnumerator
{
    float seconds;
    float startTime;

    public MyWaitForSeconds(float _seconds)
    {
        seconds = _seconds;
        startTime = Time.time;
    }

    public bool MoveNext()
    {
        return (Time.time - startTime) < seconds;
    }

    public object Current
    {
        get
        {
            return null;
        }
    }

    public void Reset()
    {
        startTime = Time.time;
    }
}
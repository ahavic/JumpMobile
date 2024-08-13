using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TimerCoroutineManager : MonoBehaviour
{
    static TimerCoroutineManager Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    #region Timer set ups

    /// <summary>
    /// Basic timer with callback at end of timer
    /// </summary>
    /// <param name="callBack"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static Coroutine SetUpTimer(Action callBack, float time)
    {
        Coroutine c = Instance.StartCoroutine(Timer(callBack, time));        
        return c;
    }

    /// <summary>
    /// Iteration timer at every yield return frame
    /// </summary>
    /// <param name="callBack"></param>
    /// <param name="time"></param>
    /// <param name="bound">A bound used in the callback (i.e for interpolation)</param>
    /// <returns></returns>
    public static Coroutine SetUpTimer(Action<float, float> callBack, float time, float bound, float delayTime = 0)
    {
        Coroutine c = Instance.StartCoroutine(Timer(callBack, time, bound, delayTime));
        return c;
    }

    /// <summary>
    /// Iteration timer at every yield return frame
    /// </summary>
    /// <typeparam name="T">the bound type used for the callback</typeparam>
    /// <param name="callBack"></param>
    /// <param name="time"></param>
    /// <param name="lowerBound">lower bound used in callback (i.e. for interpolation</param>
    /// <param name="upperBound">upper bound used in callback (i.e. for interpolation</param>
    /// <returns></returns>
    public static Coroutine SetUpTimer<T>(Action<float, T, T> callBack, float time, T lowerBound, T upperBound, float delayTime = 0)
    {
        Coroutine c = Instance.StartCoroutine(Timer(callBack, time, lowerBound, upperBound, delayTime));
        return c;
    }

    /// <summary>
    /// Iteration timer with a conditional break
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="callBack"></param>
    /// <param name="time"></param>
    /// <param name="lowerBound"></param>
    /// <param name="upperBound"></param>
    /// <param name="conditional">the conditional to check for ongoing loop</param>
    /// <param name="delayTime"></param>
    /// <returns></returns>
    public static Coroutine SetUpTimer<T>(Action<float, T, T> callBack, T lowerBound, T upperBound, Func<bool> conditional, float delayTime = 0)
    {
        Coroutine c = Instance.StartCoroutine(Timer(callBack, lowerBound, upperBound, conditional,delayTime));
        return c;
    }

    #endregion

    #region Timer loop functions

    static IEnumerator Timer(Action callBack, float time)
    {
        yield return Wait.WaitForSeconds(time);
        callBack();
    }
    
    static IEnumerator Timer(Action<float, float> callBack, float time, float bound, float delayTime = 0)
    {
        if (delayTime > 0)
            yield return Wait.WaitForSeconds(delayTime);

        float t = 0;
        while(t < time)
        {
            callBack(t, bound);
            t += Time.deltaTime;
            yield return null;
        }
    }

    static IEnumerator Timer<T>(Action<float, T, T> callBack, float time, T lowerBound, T upperBound, float delayTime = 0)
    {
        if (delayTime > 0)
            yield return Wait.WaitForSeconds(delayTime);

        float t = 0;
        while (t < time)
        {
            callBack(t, lowerBound, upperBound);
            t += Time.deltaTime;
            yield return null;
        }
    }

    static IEnumerator Timer<T>(Action<float, T, T> callBack, T lowerBound, T upperBound, Func<bool> conditional, float delayTime = 0)
    {
        if (delayTime > 0)
            yield return Wait.WaitForSeconds(delayTime);

        float t = 0;
        while (conditional())
        {
            callBack(t, lowerBound, upperBound);
            t += Time.deltaTime;
            yield return null;
        }
    }

    #endregion

    public static void DisableTimer(Coroutine c)
    {
        if (c != null)
            Instance.StopCoroutine(c);
    }
}

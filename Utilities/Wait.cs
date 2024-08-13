using System.Collections.Generic;
using UnityEngine;

public static class Wait
{
    static Dictionary<int, WaitForSeconds> caches = new Dictionary<int, WaitForSeconds>();

    public static WaitForSeconds WaitForSeconds(float k)
    {
        if (k < 0.01f || k > 999)
        {
            Debug.Log("<color=yellow><b>Seconds is out of key range, returning an uncached WaitforSeconds</b></color>");
            return new WaitForSeconds(k);
        }

        int key = GetKey(k);

        if (!caches.ContainsKey(key))
        {
            Debug.Log(caches.Count);
            caches.Add(key, new WaitForSeconds(k));
        }

        return caches[key];
    }

    static int GetKey(float k) =>
        (int)k * 100;
}

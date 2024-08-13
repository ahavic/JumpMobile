using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{    
    public static float Remap(float a, float b, float c, float d, float val)
    {
        float remapped = Mathf.InverseLerp(a, b, val);
        return Mathf.Lerp(c, d, remapped);
    }
}

public static class Vector3Extensions
{
    public static Vector3 AxisMod(this Vector3 vect, float? x = null, float? y = null, float? z = null)
    {
        vect.x = x ?? vect.x;
        vect.y = y ?? vect.y;
        vect.z = z ?? vect.z;
        return vect;
    }
}

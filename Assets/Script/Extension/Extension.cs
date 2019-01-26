using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
    public static Color With(this Color origin, float? r = null, float? g = null, float? b = null, float? a = null)
    {
        return new Color(r ?? origin.r, g ?? origin.g, b ?? origin.b, a ?? origin.a);
    }

    public static Vector3 With(this Vector3 origin, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x ?? origin.x, y ?? origin.y, z ?? origin.z);
    }
}

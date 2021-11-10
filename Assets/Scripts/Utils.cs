using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    //Makes floats boolean operable
    public static bool NumToBool(float f) { return 0 != f; }

    //Makes integers boolean operable
    public static bool NumToBool(int i) { return NumToBool((float)i); }

    //"Normalizes" floats between -1 and 1
    public static float NormalizeNum(float f) { return f == 0 ? 0 : f / Mathf.Abs(f); }

    //"Normalizes" ints between -1 and 1
    public static int NormalizeNum(int i) { return i == 0 ? 0 : i / Mathf.Abs(i); }

    public static void XYMoveRecalc(ref float x, ref float y)
    {
        if (x != 0 && y != 0)
        {
            float newVel = Mathf.Sqrt(Mathf.Pow(Mathf.Abs(x), 2) / 2);
            x = newVel * Mathf.Sign(x);
            y = newVel * Mathf.Sign(y);
        }
    }
}

using UnityEngine;

public static class MathExtensions
{
    #region Clamp

    public static int RClamp(ref this int value, int minValue, int maxValue)
    {
        return Mathf.Clamp(value, minValue, maxValue);
    }


    public static float RClamp(ref this float value, float minValue, float maxValue)
    {
        return Mathf.Clamp(value, minValue, maxValue);
    }


    public static Vector3 RClamp(ref this Vector3 value, Vector3 minValue, Vector3 maxValue)
    {
        value.x.RClamp(minValue.x, maxValue.x);
        value.y.RClamp(minValue.y, maxValue.y);
        value.z.RClamp(minValue.z, maxValue.z);
        
        return value;
    }

    #endregion

    #region Lerp

    public static float RLerp(ref this float value, float targetValue, float timer)
    {
        return Mathf.Lerp(value, targetValue, timer);
    }

    public static Vector3 RLerp(ref this Vector3 value, Vector3 targetValue, float timer)
    {
        return Vector3.Lerp(value, targetValue, timer);
    }

    #endregion

    public static float FMap(this float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return Mathf.Clamp((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow, toLow, toHigh);
    }
    public static int FMapToInt(this int value, int fromLow, int fromHigh, int toLow, int toHigh)
    {
        return Mathf.Clamp((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow, toLow, toHigh);
    }
}
using UnityEngine;

public class VectorHelpers 
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
      
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    
    public static Vector3 DegreeToVector3(float degree)
    {
        if (degree < 0)
            degree = 360 - Mathf.Abs(degree);
        var vector2= RadianToVector2(degree * Mathf.Deg2Rad);
        var vector = new Vector3(vector2.y, 0 ,vector2.x);
        return vector;
    }
}

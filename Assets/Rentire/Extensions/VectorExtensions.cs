using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions {

    public static Vector2 xy (this Vector3 v) {
        return new Vector2 (v.x, v.y);
    }

    public static Vector3 WithX (this Vector3 v, float x) {
        return new Vector3 (x, v.y, v.z);
    }

    public static Vector3 WithY (this Vector3 v, float y) {
        return new Vector3 (v.x, y, v.z);
    }

    public static Vector3 WithZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 WithXY(this Vector3 v, float x, float y)
    {
        return new Vector3(x, y, v.z);
    }
    public static Vector3 WithYZ(this Vector3 v, float y, float z)
    {
        return new Vector3(v.x, y, z);
    }
    public static Vector3 WithXZ(this Vector3 v, float x, float z)
    {
        return new Vector3(x, v.y, z);
    }

    public static Vector2 WithX (this Vector2 v, float x) {
        return new Vector2 (x, v.y);
    }

    public static Vector2 WithY (this Vector2 v, float y) {
        return new Vector2 (v.x, y);
    }

    public static Vector3 WithZ (this Vector2 v, float z) {
        return new Vector3 (v.x, v.y, z);
    }

    public static Vector3 AddX (this Vector3 v, float x) {
        return v + Vector3.right * x;
    }
    public static Vector3 AddY (this Vector3 v, float y) {
        return v + Vector3.up * y;
    }
    public static Vector3 AddZ (this Vector3 v, float z) {
        return v + Vector3.forward * z;
    }
    
    public static Vector2 AddX (this Vector2 v, float x) {
        return v + Vector2.right * x;
    }
    public static Vector2 AddY (this Vector2 v, float y) {
        return v + Vector2.up * y;
    }

    // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnAxis (this Vector3 axisDirection, Vector3 point, bool isNormalized = false) {
        if (!isNormalized) axisDirection.Normalize ();
        var d = Vector3.Dot (point, axisDirection);
        return axisDirection * d;
    }

    // lineDirection - unit vector in direction of line
    // pointOnLine - a point on the line (allowing us to define an actual line in space)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnLine (
        this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false) {
        if (!isNormalized) lineDirection.Normalize ();
        var d = Vector3.Dot (point - pointOnLine, lineDirection);
        return pointOnLine + (lineDirection * d);
    }

    public static Vector4 EulerToQuaternion (Vector3 p) {
        p.x *= Mathf.Deg2Rad;
        p.y *= Mathf.Deg2Rad;
        p.z *= Mathf.Deg2Rad;
        Vector4 q;
        float cy = Mathf.Cos (p.z * 0.5f);
        float sy = Mathf.Sin (p.z * 0.5f);
        float cr = Mathf.Cos (p.y * 0.5f);
        float sr = Mathf.Sin (p.y * 0.5f);
        float cp = Mathf.Cos (p.x * 0.5f);
        float sp = Mathf.Sin (p.x * 0.5f);
        q.w = cy * cr * cp + sy * sr * sp;
        q.x = cy * cr * sp + sy * sr * cp;
        q.y = cy * sr * cp - sy * cr * sp;
        q.z = sy * cr * cp - cy * sr * sp;
        return q;
    }

    public static Vector3 QuaternionToEuler (Vector4 p) {
        Vector3 v;
        Vector4 q = new Vector4 (p.w, p.z, p.x, p.y);
        v.y = Mathf.Atan2 (2f * q.x * q.w + 2f * q.y * q.z, 1 - 2f * (q.z * q.z + q.w * q.w));
        v.x = Mathf.Asin (2f * (q.x * q.z - q.w * q.y));
        v.z = Mathf.Atan2 (2f * q.x * q.y + 2f * q.z * q.w, 1 - 2f * (q.y * q.y + q.z * q.z));
        v *= Mathf.Rad2Deg;
        v.x = v.x > 360 ? v.x - 360 : v.x;
        v.x = v.x < 0 ? v.x + 360 : v.x;
        v.y = v.y > 360 ? v.y - 360 : v.y;
        v.y = v.y < 0 ? v.y + 360 : v.y;
        v.z = v.z > 360 ? v.z - 360 : v.z;
        v.z = v.z < 0 ? v.z + 360 : v.z;
        return v;
    }
    public static int Clamp(this int val, int min, int max)
    {
        return Mathf.Clamp(val, min, max);
    }
}
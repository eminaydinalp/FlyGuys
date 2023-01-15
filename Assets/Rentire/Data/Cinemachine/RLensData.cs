using UnityEngine;

public class RLensData
{
    public RLensData(float focalLength, float nearClipPlane, float farClipPlane, Vector2 lensShift, float dutch)
    {
        FocalLength = focalLength;
        NearClipPlane = nearClipPlane;
        FarClipPlane = farClipPlane;
        LensShift = lensShift;
        Dutch = dutch;
    }

    public float FocalLength { get; set; }
    public float NearClipPlane { get; set; }
    public float FarClipPlane { get; set; }
    public Vector2 LensShift { get; set; }
    public float Dutch { get; set; }
}
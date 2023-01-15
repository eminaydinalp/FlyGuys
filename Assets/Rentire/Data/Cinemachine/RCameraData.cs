using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RCameraData<T1, T2, T3>
{
    public RCameraData(T1 aimData, T2 followData, T3 lensData)
    {
        AimData = aimData;
        FollowData = followData;
        LensData = lensData;
    }

    public T1 AimData { get; set; }
    public T2 FollowData { get; set; }
    public T3 LensData { get; set; }
}
using System;
using System.Diagnostics;
using Dreamteck.Splines;
using PathCreation;

public class Level : RMonoBehaviour
{
    public int LevelColorNo = 0;
    public SplineComputer splineComputer;

    private void Awake()
    {
        splineComputer = GetComponentInChildren<SplineComputer>();
    }
    
    public void AssignSpline(ref SplineFollower follower)
    {
        follower.spline = splineComputer;
    }
}
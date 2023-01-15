using Dreamteck.Splines;
using Sirenix.OdinInspector;
using UnityEngine;

public class RSplineBase : RMonoBehaviour
{
    public SplineComputer splineComputer;

    [Button()]
    public void FindSplineComputer()
    {
        splineComputer = GameObject.FindObjectOfType<SplineComputer>();
    }
}

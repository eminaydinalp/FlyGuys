using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectPositioner : RSplineBase
{
    public bool lookBack = false;

    [Button("Update Position")]
    public void UpdatePosition()
    {
        if (splineComputer)
        {
            var sample = splineComputer.Project(transform.position);

            transform.SetPositionAndRotation(sample.position,
                lookBack ? sample.rotation * Quaternion.AngleAxis(180, Vector3.up) : sample.rotation);
        }
    }

    [Button]
    public void PutToGround()
    {
        var raycasthit = new RaycastHit[1];
        var ray = new Ray(transform.position, Vector3.down);
        int size = Physics.RaycastNonAlloc(ray, raycasthit, Mathf.Infinity, Layers.GROUND);

        if (size > 0)
            transform.position = raycasthit[0].point;
    }
}
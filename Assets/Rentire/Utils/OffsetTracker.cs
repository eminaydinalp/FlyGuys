using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using UnityEngine;

public class OffsetTracker : TransformObject
{
    public Transform TargetObject;

    public Vector3 Offset;
    public bool TrackRotation = false;

    public bool startWithWorldOffset = false;

    private Quaternion _rotationOffset;
    // Start is called before the first frame update
    void Start()
    {
        if(!TargetObject)
            Log.Error("Target Object is not assigned!");
        //TargetObject = PlayerController.Instance.PlayerTransform;
        if (startWithWorldOffset)
        {
            Offset = Transform.position - TargetObject.position;
        }

        if (TrackRotation)
        {
            _rotationOffset = Transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform.position = TargetObject.position + Offset;
        if (TrackRotation)
        {
            Transform.rotation = TargetObject.rotation * _rotationOffset;
        }
    }
}

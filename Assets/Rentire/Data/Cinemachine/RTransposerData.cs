using Cinemachine;
using UnityEngine;

public class RTransposerData
{
    public RTransposerData(CinemachineTransposer.BindingMode bindingMode, Vector3 followOffset, float xDamping, float yDamping, float zDamping, float yawDamping)
    {
        BindingMode = bindingMode;
        FollowOffset = followOffset;
        XDamping = xDamping;
        YDamping = yDamping;
        ZDamping = zDamping;
        YawDamping = yawDamping;
    }

    public CinemachineTransposer.BindingMode BindingMode { get; set; }
    public Vector3 FollowOffset { get; set; }
    public float XDamping { get; set; }
    public float YDamping { get; set; }
    public float ZDamping { get; set; }
    public float YawDamping { get; set; }
}
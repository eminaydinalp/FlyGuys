using UnityEngine;

public class RComposerData
{
    public RComposerData(Vector3 trackedObjectOffset, float lookAheadTime, float lookAheadSmoothing, bool lookAheadIgnoreY, float horizontalDamping, float verticalDamping)
    {
        TrackedObjectOffset = trackedObjectOffset;
        LookAheadTime = lookAheadTime;
        LookAheadSmoothing = lookAheadSmoothing;
        LookAheadIgnoreY = lookAheadIgnoreY;
        HorizontalDamping = horizontalDamping;
        VerticalDamping = verticalDamping;
    }

    public Vector3 TrackedObjectOffset { get; set; }
    public float LookAheadTime { get; set; }
    public float LookAheadSmoothing { get; set; }
    public bool LookAheadIgnoreY { get; set; }
    public float HorizontalDamping { get; set; }
    public float VerticalDamping { get; set; }
}
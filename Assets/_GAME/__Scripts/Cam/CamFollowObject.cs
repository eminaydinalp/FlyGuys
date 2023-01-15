using UnityEngine;

public class CamFollowObject : TransformObject
{
    public UpdateType updateType;
    private Transform _playerTransform => player.playerTransform;

    private void FixedUpdate()
    {
        UpdateTransform(UpdateType.FixedUpdate);
    }

    private void Update()
    {
        UpdateTransform(UpdateType.Update);
    }

    private void LateUpdate()
    {
        UpdateTransform(UpdateType.LateUpdate);
    }

    private void UpdateTransform(UpdateType updateType)
    {
        if(updateType != this.updateType)
            return;
        
        Transform.SetPositionAndRotation(_playerTransform.position, _playerTransform.rotation);
    }
}

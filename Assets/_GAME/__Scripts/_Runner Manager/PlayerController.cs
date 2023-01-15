using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using Rentire.Core;
using UnityEngine;
using Lean.Touch;

public class PlayerController : Singleton<PlayerController>
{
    public PlayerControlType PlayerControlType;

    public GameObject Player;
    public Transform PlayerTransform;
    public IPlayerMove PlayerMove;
    public IPlayerSlide PlayerSlide;

    public SplineFollower follower;

    public float PlayerSpeed = 5;
    public float MovementFactor = 2f;
    private float screenMovement => InputManager.Instance.GetScreenDelta().x;

    private CamFollowObject camFollowObject;

    private void Awake()
    {
        PlayerMove = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<IPlayerMove>();
        PlayerTransform = Player.transform;
        camFollowObject = FindObjectOfType<CamFollowObject>();
    }


    private void FixedUpdate()
    {
        if (gameState != GameState.Running)
            return;

        float xUnit = Fmap(screenMovement, -Screen.width, Screen.width, -1 * MovementFactor, 1 * MovementFactor);
        
        // follower.motion.offset
        //
        // PlayerMove.Move(Path, PlayerSpeed, xUnit);
        //
        // var pathPosition = Path.GetClosestPointOnPath(PlayerTransform.position);

        // camFollowObject.UpdateTransform(pathPosition, PlayerTransform.rotation);
    }
}

public enum PlayerControlType
{
    Spline,
    Forward
}

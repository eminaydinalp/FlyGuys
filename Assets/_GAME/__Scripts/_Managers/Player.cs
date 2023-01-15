using Rentire.Core;
using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerStateManager PlayerStateManager;

    public PlayerAnimator playerAnimator;
    public Transform playerTransform;

    public Rigidbody playerRigidbody;
    public Collider playerCollider;
}
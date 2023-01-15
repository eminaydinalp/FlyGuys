using System;
using Dreamteck.Splines;
using Rentire.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Screen = UnityEngine.Device.Screen;

public class PlayerMover : RigidbodyObject, IPlayerStateObserver, IGameStateObserver
{
    public SplineFollower splineFollower;
    public Transform localMover;

    public bool isStop;
    private float screenMovement => InputManager.Instance.GetScreenDelta().x;

    [SerializeField] private float mPlayerSpeed = 7;

    public float playerSpeed
    {
        get => mPlayerSpeed;
        set
        {
            mPlayerSpeed = value;
            //splineFollower.followSpeed = playerSpeed;
        }
    }

    private float currentSpeed;

    public float MovementFactor = 10f;

    public float normalizedTime = 0f;

    private float multiplier = 0f;

    public float maxXLimit = 2.5f;

    private float _prevXUnit;
    public float xUnit;
    private float my_xUnit;

    public AnimationCurve Interpolation;
    float currentSkinDirection;
    [SerializeField] Transform PlayerSkin;

    private float baseSpeed;
    public float slowDownSpeed = 3.5f;

    public float CrashTimer = 0f;
    public float CrashSpeed = 15;

    protected override void Awake()
    {
        base.Awake();
        AddToGameObserverList();
        AddToPlayerStateManager();
    }
    

    private void Start()
    {
        GetComponentAndAssign(ref splineFollower);
        LevelManager.Instance.CurrentLevel.AssignSpline(ref splineFollower);
        splineFollower.autoStartPosition = true;
        splineFollower.followSpeed = 0f;
        splineFollower.follow = true;


        if (splineFollower.offsetModifier.keys != null && splineFollower.offsetModifier.keys.Length > 0)
            splineFollower.offsetModifier.keys[0].interpolation = Interpolation;
        baseSpeed = playerSpeed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (gameState is GameState.Running or GameState.Final)
        {
            if (currentPlayerState == PlayerState.Running)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, playerSpeed, Time.fixedDeltaTime * 4);
            }
            else if (currentPlayerState == PlayerState.SlowingDown)
            {
                currentSpeed = Mathf.Lerp(currentSpeed, slowDownSpeed, Time.fixedDeltaTime * 5);
            }

            //splineFollower.followSpeed = Mathf.Abs(currentSpeed);
            //CrashControl();
            MoveX();
        }
    }


    [Button("Set Crash")]
    public void SetCrash(float timer = 0.35f)
    {
        CrashTimer = timer;
    }

    void CrashControl()
    {
        CrashTimer -= Time.fixedDeltaTime;
        if (CrashTimer > 0)
        {
            currentSpeed = -CrashSpeed;
            playerSpeed = -CrashSpeed;
            splineFollower.applyDirectionRotation = false;
            splineFollower.direction = Spline.Direction.Backward;
        }
        else
        {
            playerSpeed = baseSpeed;
            splineFollower.applyDirectionRotation = true;
            splineFollower.direction = Spline.Direction.Forward;
        }
    }

    void MoveX()
    {
        xUnit = Fmap(screenMovement, -Screen.width, +Screen.width, -1 * MovementFactor, MovementFactor);
        my_xUnit += xUnit;
        my_xUnit = Mathf.Clamp(my_xUnit, -maxXLimit, maxXLimit);
        RotateSkin(xUnit);
        //splineFollower.motion.offset = splineFollower.motion.offset.WithX(my_xUnit);

        if (GameManager.Instance.isFinal)
        {
            localMover.transform.localPosition = Vector3.Lerp(localMover.transform.localPosition,
                new Vector3(0,
                    localMover.transform.localPosition.y,
                    localMover.transform.localPosition.z), 10 * Time.deltaTime);
        }
        else
        {
            localMover.transform.localPosition = new Vector3(my_xUnit,
                localMover.transform.localPosition.y,
                localMover.transform.localPosition.z);
        }
    }

    void RotateSkin(float xUnit)
    {
        if (GameManager.Instance.isFinal)
        {
            PlayerSkin.localEulerAngles = Vector3.zero;
            return;
        }
        currentSkinDirection += xUnit * 20;
        currentSkinDirection = Mathf.Clamp(currentSkinDirection, -25, 25);
        PlayerSkin.localRotation = Quaternion.Euler(PlayerSkin.localEulerAngles.x, currentSkinDirection, PlayerSkin.localEulerAngles.z);
        currentSkinDirection = Mathf.Lerp(currentSkinDirection, 0, Time.fixedDeltaTime * 5);
    }

    public void AddToPlayerStateManager()
    {
        player.PlayerStateManager.AddListener(this);
    }

    public void OnPlayerStateChanged()
    {
        if(isStop)return;
        if (GameManager.Instance.isFinal)
        {
            splineFollower.followSpeed = 2f;
            return;
        }
        switch (currentPlayerState)
        {
            case PlayerState.Idle:
                splineFollower.followSpeed = 0f;
                break;
            case PlayerState.Running:
                Log.Info("Player is moving");
                splineFollower.followSpeed = playerSpeed;
                splineFollower.follow = true;
                break;
            case PlayerState.Finish:
                Log.Info("Player is at Finish State");
                splineFollower.followSpeed = 0f;
                break;
            case PlayerState.SlowingDown:
                splineFollower.followSpeed = playerSpeed / 2f;
                break;
            case PlayerState.Win:
                PlayerSkin.localRotation = Quaternion.Euler(0, 180, 0);
                break;
        }
    }

    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }

    public void OnGameStateChanged()
    {
        if (gameState == GameState.Fail)
        {
            playerSpeed = 0;
            splineFollower.followSpeed = 0;
        }
    }
}
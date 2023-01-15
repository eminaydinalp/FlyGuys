using Lean.Touch;
using Rentire.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public bool canGetInput = true;

    public bool CanGetInput
    {
        get => canGetInput;
        set
        {
            canGetInput = value;
            if (!canGetInput)
            {
                FingerUp(myFinger);
            }
        }
    }
    public bool canBeUsedWithKeyboard = false;
    public float keyboardMultiplier = 100;
    private float _keyboardInput;
    public float MovementFactor = 1f;
    public bool showInputDelta;
    public bool showTimer;

    [SerializeField] private float xScreenPositionDistance;

    private float delta;
    private float timer;
    
    

    private Vector3 firstTouchPosition;
    private LeanFinger myFinger;
    private bool fingerActive => myFinger != null;
    private float screenHeight => Screen.height;
    private float screenWidth => Screen.width;

    private void Awake()
    {
        MovementFactor = RemoteManager.Instance.GetPlayerMovementFactor();
#if !UNITY_EDITOR
        showTimer = false;
        showInputDelta = false;
        canBeUsedWithKeyboard = false;
#endif
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (gameState == GameState.Running)
        {
            timer += Time.deltaTime;
        }

        if(Keyboard.current[Key.R].wasPressedThisFrame)
        {
            LevelManager.Instance.RestartLevel();
        }

        if (canBeUsedWithKeyboard)
        {
            if (Keyboard.current[Key.A].isPressed)
                _keyboardInput = -1f;
            else if(Keyboard.current[Key.D].isPressed)
                _keyboardInput = 1f;
            else
            {
                if(Keyboard.current[Key.S].wasPressedThisFrame)
                {
                    RunGame();
                }
                _keyboardInput = 0f;
            }
        
        }
    }
#endif
    private void OnEnable()
    {
        LeanTouch.OnFingerDown += FingerDown;
        LeanTouch.OnFingerUpdate += FingerUpdate;
        LeanTouch.OnFingerUp += FingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= FingerDown;
        LeanTouch.OnFingerUpdate -= FingerUpdate;
        LeanTouch.OnFingerUp -= FingerUp;
    }

    private void OnGUI()
    {
        ShowInputDelta();
        ShowTimer();
    }

    void ShowTimer()
    {
        if (!showTimer)
            return;

        var style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.red;

        GUI.Label(new Rect(10, Screen.height - 60, 800, 200), "TIME : " + timer.ToString("F1"), style);
    }

    void ShowInputDelta()
    {
        if (!showInputDelta)
            return;

        var style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.red;

        GUI.Label(new Rect(10, 40, 800, 200), "DELTA : " + delta, style);
    }

    public Vector2 GetSwipeScreenDelta()
    {
        if (!fingerActive)
            return Vector2.zero;

        return myFinger.SwipeScreenDelta;
    }

    public Vector2 GetScreenDelta()
    {
        var vector = !canBeUsedWithKeyboard
            ? (!fingerActive ? Vector2.zero : myFinger.ScreenDelta * MovementFactor)
            : Vector2.zero.WithX(_keyboardInput * keyboardMultiplier);
        return vector;
    }

    public Vector2 GetScreenPosition(float multiplier = 1f)
    {
        if (fingerActive)
            xScreenPositionDistance = (multiplier * ((Vector2)firstTouchPosition - myFinger.ScreenPosition)).x;
        return !fingerActive ? Vector2.zero : multiplier * (myFinger.ScreenPosition - (Vector2)firstTouchPosition);
    }

    public void UpdateFirstTouchPosition()
    {
        if (fingerActive)
            firstTouchPosition = myFinger.ScreenPosition;
    }

    public Vector3 GetWorldDelta(float distance = 10f)
    {
        if (!fingerActive)
            return Vector3.zero;

        return myFinger.GetWorldDelta(distance) * MovementFactor;
    }

    public bool GetIfFingerActive()
    {
        return fingerActive;
    }


    private void RunGame()
    {
        if (gameState == GameState.WaitingToStart) gameManager.SetGameRunning();
    }

    private void FingerDown(LeanFinger finger)
    {
        if (!canGetInput)
            return;

        if (finger.IsOverGui)
            return;

        if (myFinger == null)
        {
            firstTouchPosition = finger.ScreenPosition;
            myFinger = finger;
            RunGame();
        }
    }

    private void FingerUpdate(LeanFinger finger)
    {
        if (finger == myFinger) delta = finger.SwipeScaledDelta.x;
    }

    private void FingerUp(LeanFinger finger)
    {
        if (myFinger == finger) myFinger = null;
    }
}
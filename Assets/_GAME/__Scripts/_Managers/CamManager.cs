using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine;
using Rentire.Core;
using UnityEngine;

public class CamManager : Singleton<CamManager>,IGameStateObserver
{
    private ObservedValue<CameraState> CurrentCameraState = new ObservedValue<CameraState>(CameraState.Idle);

    [SerializeField] CinemachineVirtualCamera runCamera;
    [SerializeField] CinemachineVirtualCamera startCamera;
    [SerializeField] CinemachineVirtualCamera finalCamera;
    [SerializeField] CinemachineVirtualCamera winCamera;

    public Camera UICamera;

    public Canvas MasterCanvas;

    private List<CinemachineVirtualCamera> _cameras = new List<CinemachineVirtualCamera>();

    void Awake()
    {
        AddToGameObserverList();
        if (!MasterCanvas)
        {
            MasterCanvas = GameObject.FindWithTag("MasterCanvas").GetComponent<Canvas>();
        }

        if (!UICamera)
        {
            UICamera = GameObject.FindWithTag("UICamera").GetComponent<Camera>();
        }
        ReflectionHelpers<CinemachineVirtualCamera>.CastFieldsIntoList(ref _cameras,Instance,BindingFlags.NonPublic);

        CameraChange(startCamera);
    }


    public void CameraChange(CinemachineVirtualCamera camera, bool isOn = true)
    {
        for (int i = 0; i < _cameras.Count; i++)
        {
            _cameras[i].gameObject.SetActive(false);
        }
        camera.gameObject.SetActive(isOn);
    }


    #region States Changed

    public void OnGameStateChanged()
    {
        if (gameState == GameState.WaitingToStart)
            SetCameraStateIdle();


        if (gameState == GameState.Running)
            SetCameraStateRun();


        if (gameState == GameState.Success)
            SetCameraStateWin();

        if (gameState == GameState.Final)
            SetCameraStateFinal();
    }


    void CameraStateChanged()
    {
        var state = CurrentCameraState.Value;

        if (state == CameraState.Idle)
            CameraChange(startCamera);


        if (state == CameraState.Run)
        {
            CameraChange(runCamera);
        }

        if (state == CameraState.Final)
            CameraChange(finalCamera);


        if (state == CameraState.Win)
            CameraChange(winCamera);
    }

    #endregion

    #region Camera State Controls

    public void SetCameraStateRun()
    {
        CurrentCameraState.Value = CameraState.Run;
    }

    public void SetCameraStateIdle()
    {
        CurrentCameraState.Value = CameraState.Idle;
    }


    public void SetCameraStateFinal()
    {
        CurrentCameraState.Value = CameraState.Final;
    }

    public void SetCameraStateWin()
    {
        CurrentCameraState.Value = CameraState.Win;
    }

    public void SetCameraState(CameraState state)
    {
        CurrentCameraState.Value = state;
    }

    #endregion


    #region Enable / Disable / State

    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }

    private void OnEnable()
    {
        CurrentCameraState.OnValueChange += CameraStateChanged;
    }


    private void OnDisable()
    {
        CurrentCameraState.OnValueChange -= CameraStateChanged;
    }
    #endregion

}
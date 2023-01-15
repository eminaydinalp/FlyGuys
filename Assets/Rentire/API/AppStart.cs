using System;
using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;

public class AppStart : RMonoBehaviour
{
    public float TimeScale = 1f;
    bool isFirstFrame = true;
    void Awake()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
        Time.timeScale = TimeScale;
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }
}

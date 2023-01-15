using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if DOTWEEN_API
using DG.Tweening;
#endif
using Lean.Pool;
using System;

public class FadeOutTextMeshProUGUI : MonoBehaviour, IPoolable
{
    [Header("Auto Fade Out")]
    public bool AutoFadeOut = true;
    public float AutoFadeOutTime = .5f;
    public float AutoFadeOutDelay = .5f;


    private TextMeshProUGUI TextToFadeOut;
    private Color colorAtStart;
    private void Awake()
    {
        TextToFadeOut = GetComponent<TextMeshProUGUI>();
        
    }
    public void FadeOut(Action OnFadeOutFinished = null, float second = 0f, float delay = 0f)
    {
        colorAtStart = TextToFadeOut.color;
        #if DOTWEEN_API
        TextToFadeOut.DOColor(new Color(colorAtStart.r, colorAtStart.g, colorAtStart.b, 0f), second).SetDelay(delay).OnComplete(()=> {
            if (OnFadeOutFinished != null)
                OnFadeOutFinished.Invoke();
        });
        #endif
    }



    public void OnSpawn()
    {
        if(AutoFadeOut)
        {
            FadeOut(null, AutoFadeOutTime, AutoFadeOutDelay);
        }
    }

    public void OnDespawn()
    {
        TextToFadeOut.color = colorAtStart;
    }

    private void OnDestroy()
    {
        TextToFadeOut.color = colorAtStart;
    }
}

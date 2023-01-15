using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRaycastUpdater : MonoBehaviour
{
    public bool IsInteractable = false;
    public bool IsRaycastBlocked = false;
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = IsInteractable;
        canvasGroup.blocksRaycasts = IsRaycastBlocked;
    }

    private void OnEnable()
    {
        if(canvasGroup != null) { 
            canvasGroup.interactable = IsInteractable;
            canvasGroup.blocksRaycasts = IsRaycastBlocked;
        }
    }
}

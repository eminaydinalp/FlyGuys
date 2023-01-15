using System.Collections;
using System.Collections.Generic;
#if DOTWEEN_API
using DG.Tweening;
#endif

using Rentire.Core;
using TMPro;
using UnityEngine;

public class UITextUpdater : Singleton<UITextUpdater>
{
    public GameObject TextPrefab;
    public Camera UICamera;
    private Transform _transform;
    private void Awake()
    {
        _transform = transform;
    }

    public void CreateTextOnScreen(string text, Vector3 worldPosition, bool animate = true)
    {
        //var screenPoint = Camera.main.WorldToScreenPoint(worldPosition);

        var textGO = Lean.Pool.LeanPool.Spawn(TextPrefab, _transform, false);
        var clone = textGO.GetComponent<RectTransform>();

        clone.anchorMin = Camera.main.WorldToViewportPoint(worldPosition);
        clone.anchorMax = clone.anchorMin;

        clone.anchoredPosition = clone.localPosition;

        clone.anchorMin = new Vector2(0.5f, 0.5f);
        clone.anchorMax = clone.anchorMin;
        /*
        //rectTransform.localPosition = screenPoint;

        RectTransform CanvasRect = _transform.GetComponent<RectTransform>();

        //then you calculate the position of the UI element
        //0,0 for the canvas is at the center of the screen, whereas WorldToViewPortPoint treats the lower left corner as 0,0. Because of this, you need to subtract the height / width of the canvas * 0.5 to get the correct position.

        Vector2 ViewportPosition = UICamera.WorldToViewportPoint(worldPosition);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * 0.5f)));

        //now you can set the position of the ui element
        clone.anchoredPosition = WorldObject_ScreenPosition;
        */
        textGO.GetComponent<TextMeshProUGUI>().text = text;

        if(animate)
        {
#if DOTWEEN_API
            clone.DOLocalMoveY(50f, .5f, false).SetRelative(true).Play();
#endif

        }
    }
}

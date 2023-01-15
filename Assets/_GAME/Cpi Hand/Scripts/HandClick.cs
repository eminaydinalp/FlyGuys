using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandClick : TransformObject
{
    public bool isOn = true;

    private Canvas myCanvas;
    private Animator Animator;

    private const string CLICK = "Click";
    public Vector3 offset;

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();
        myCanvas = GetComponentInParent<Canvas>();
        gameObject.SetActive(isOn);
#if !UNITY_EDITOR
        gameObject.SetActive(false);
#endif
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Animator.SetTrigger(CLICK);
        }
    }

    void LateUpdate()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform,
            Input.mousePosition + offset, myCanvas.worldCamera, out pos);
        Transform.position = myCanvas.transform.TransformPoint(pos);
    }
}
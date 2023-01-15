using System;
using Rentire.Core;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class ObjectPositionerAndBender : RSplineBase
{
#if UNITY_EDITOR
    public Transform testTransform;

    public GameObject objectPrefab;
    public AnimationCurve curve;
    [SerializeField] private Transform[] children;
    [SerializeField] private Transform[] childrenMirror;

    [Tooltip("Objelerin curve'e göre 0..1 araliginda x ekseni carpani")]
    public float xOffsetCurveMultiplier;

    [Tooltip("Objelerin birbirleri arasindaki uzaklik")]
    public float zDistance;

    public int count;
    public bool createMirror;
    public bool useChildren;
    public bool rotateMirror;


    [Button("Update Child Positions")]
    public void UpdateChildPositions()
    {
        GetChildren();
        ArrangePositions();
        ArrangeSplinePositions();
    }

    void GetChildren()
    {
        if (useChildren)
        {
            if (createMirror && transform.childCount % 2 != 0)
                DestroyImmediate(transform.GetChild(0).gameObject);

            if (createMirror)
            {
                var childCount = transform.childCount;
                children = new Transform[childCount / 2];
                childrenMirror = new Transform[childCount / 2];
                int index = 0;
                for (int i = 0; i < childCount; i++)
                {
                    if (i % 2 == 0)
                        children[index] = transform.GetChild(i);
                    else
                    {
                        childrenMirror[index] = transform.GetChild(i);
                        index++;
                    }
                }
            }
            else
            {
                children = transform.RGetComponentsInFirstChildren<Transform>();
            }
        }
        else
        {
            if (!objectPrefab)
            {
                Log.Error("Prefab is not assigned");
                return;
            }

            DestroyExistingChildren();
            children = new Transform[count];
            if (createMirror)
                childrenMirror = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                var child = (PrefabUtility.InstantiatePrefab(objectPrefab, transform) as GameObject)?.transform;
                children[i] = child;
                if (createMirror)
                {
                    var childMirror = (PrefabUtility.InstantiatePrefab(objectPrefab, transform) as GameObject)
                        ?.transform;
                    childrenMirror[i] = childMirror;
                }
            }
        }
    }

    void ArrangePositions()
    {
        float interval = children.Length > 1 ? (1f / (children.Length - 1)) : 0f;
        double percentage = 0f;
        for (int i = 0; i < children.Length; i++)
        {
            var child = children[i];

            var position = child.position;
            Log.Info("Child position : " + position);
            Log.Info("Curve position : " + curve.Evaluate(0f));
            // Arrange z position
            position = Vector3.forward * i * zDistance;
            // Arrange x position
            position += Vector3.right * curve.Evaluate(interval * i) * xOffsetCurveMultiplier;
            Log.Info("Evaluated position : " + position);
            child.position = position;

            if (createMirror)
            {
                var childMirror = childrenMirror[i];

                var positionMirror = childMirror.position;
                // Arrange z position
                positionMirror = Vector3.forward * i * zDistance;
                // Arrange x position
                positionMirror += Vector3.left * curve.Evaluate(interval * i) * xOffsetCurveMultiplier;
                Log.Info("Evaluated position : " + positionMirror);
                childMirror.position = positionMirror;

                if (rotateMirror)
                    childMirror.rotation *= Quaternion.AngleAxis(180, Vector3.up);
            }
        }
    }

    [Button("Arrange positions on spline")]
    void ArrangeSplinePositions()
    {
        double percentage = 0f;
        float interval = children.Length > 1 ? (1f / (children.Length - 1)) : 0f;
        GetChildren();
        if (splineComputer)
        {
            for (int i = 0; i < children.Length; i++)
            {
                var child = children[i];
                if (i == 0)
                {
                    child.localPosition = Vector3.zero;
                    var projected = splineComputer.Project(child.position);
                    percentage = projected.percent;
                }

                var project = splineComputer.Evaluate(percentage);
                child.rotation = project.rotation;
                child.position = project.position +
                                 project.right * curve.Evaluate(interval * i) * xOffsetCurveMultiplier;

                if (createMirror)
                {
                    var childMirror = childrenMirror[i];
                    childMirror.rotation = project.rotation;
                    childMirror.position = project.position
                                           - project.right * curve.Evaluate(interval * i) * xOffsetCurveMultiplier;
                    if (rotateMirror)
                        childMirror.rotation *= Quaternion.AngleAxis(180, Vector3.up);
                }

                percentage = splineComputer.Travel(percentage, zDistance);
            }
        }
    }

    [Button]
    void PutToGround()
    {
        var raycasthit = new RaycastHit[1];
        
        children = transform.RGetComponentsInFirstChildren<Transform>();
        
        foreach (var child in children)
        {
            var ray = new Ray(child.position, Vector3.down);
            int size = Physics.RaycastNonAlloc(ray, raycasthit, Mathf.Infinity, Layers.GROUND);

            if (size > 0)
                child.position = raycasthit[0].point;
        }
    }

    [Button("TEST")]
    void TestSplinePosition()
    {
        var percent = splineComputer.Project(testTransform.position).percent;
        var sample = splineComputer.Evaluate(percent);

        var newPercent = splineComputer.Travel(percent, zDistance);

        testTransform.position = splineComputer.EvaluatePosition(newPercent);

        Log.Info("Percentage : " + percent);
        Log.Info("SAMPLE : " + sample.forward);
        Log.Info("New Percentage : " + newPercent);
    }

    void DestroyExistingChildren()
    {
        if (children != null && children.Length > 0)
            for (int i = children.Length - 1; i >= 0; i--)
            {
                if (children[i] != null)
                    DestroyImmediate(children[i].gameObject);
            }

        if (childrenMirror != null && childrenMirror.Length > 0)
            for (int i = childrenMirror.Length - 1; i >= 0; i--)
            {
                if (childrenMirror[i] != null)
                    DestroyImmediate(childrenMirror[i].gameObject);
            }

        children = Array.Empty<Transform>();
        childrenMirror = Array.Empty<Transform>();
    }
#endif
}
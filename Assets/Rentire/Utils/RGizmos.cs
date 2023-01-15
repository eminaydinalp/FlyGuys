using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rentire.Core;
using UnityEngine;

public class RGizmos : RMonoBehaviour
{
    public enum GizmoType
    {
        Always,
        OnlyWhenHelperSelected
    }

    public GizmoType gizmoType;

    static List<Tuple<string, Vector3, float, Color>> SphereTuple;
    static List<Tuple<string, Vector3, float, Color>> SphereWireTuple;
    static List<Tuple<string, Vector3, Vector3, Color>> LineTuple;

    private void Awake()
    {
        SphereTuple = null;
        SphereWireTuple = null;
        LineTuple = null;
    }
    public static void DrawSphere(string tag, Vector3 center, float radius, Color color = default)
    {
        if (SphereTuple == null)
            SphereTuple = new List<Tuple<string, Vector3, float, Color>>();

        var tuple = new Tuple<string, Vector3, float, Color>(tag, center, radius, color);

        if (SphereTuple.Any(x => x.Item1.Equals(tag)))
        {
            var listIndex = SphereTuple.FindIndex(x => x.Item1.Equals(tag));
            SphereTuple[listIndex] = tuple;
        }
        else
            SphereTuple.Add(tuple);
    }

    public static void DrawWireSphere(string tag, Vector3 center, float radius, Color color = default)
    {
        if (SphereWireTuple == null)
            SphereWireTuple = new List<Tuple<string, Vector3, float, Color>>();

        var tuple = new Tuple<string, Vector3, float, Color>(tag, center, radius, color);

        if (SphereWireTuple.Any(x => x.Item1.Equals(tag)))
        {
            var listIndex = SphereWireTuple.FindIndex(x => x.Item1.Equals(tag));
            SphereWireTuple[listIndex] = tuple;
        }
        else
            SphereWireTuple.Add(tuple);
    }

    public static void DrawLine(string tag, Vector3 from, Vector3 to, Color color = default)
    {
        if (LineTuple == null)
            LineTuple = new List<Tuple<string, Vector3, Vector3, Color>>();

        var tuple = new Tuple<string, Vector3, Vector3, Color>(tag, from, to, color);

        if (LineTuple.Any(x => x.Item1.Equals(tag)))
        {
            var listIndex = LineTuple.FindIndex(x => x.Item1.Equals(tag));
            LineTuple[listIndex] = tuple;
        }
        else
            LineTuple.Add(tuple);
    }

    private void OnDrawGizmosSelected()
    {
        if (gizmoType == GizmoType.OnlyWhenHelperSelected)
            DrawGizmos();
    }

    private void OnDrawGizmos()
    {
        if (gizmoType == GizmoType.Always)
            DrawGizmos();
    }

    void DrawGizmos()
    {
        if (SphereTuple != null)
        {
            for (int i = 0; i < SphereTuple.Count; i++)
            {
                if (SphereTuple[i].Item4 == default)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = SphereTuple[i].Item4;
                Gizmos.DrawSphere(SphereTuple[i].Item2, SphereTuple[i].Item3);
            }

        }

        if (SphereWireTuple != null)
        {
            for (int i = 0; i < SphereWireTuple.Count; i++)
            {
                if (SphereWireTuple[i].Item4 == default)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = SphereWireTuple[i].Item4;
                Gizmos.DrawWireSphere(SphereWireTuple[i].Item2, SphereWireTuple[i].Item3);
            }

        }

        if (LineTuple != null)
        {
            for (int i = 0; i < LineTuple.Count; i++)
            {
                if (LineTuple[i].Item4 == default)
                    Gizmos.color = Color.red;
                else
                    Gizmos.color = LineTuple[i].Item4;
                Gizmos.DrawLine(LineTuple[i].Item2, LineTuple[i].Item3);
            }
        }
    }
    private void OnDisable()
    {
        SphereTuple = null;
        SphereWireTuple = null;
        LineTuple = null;
    }
}

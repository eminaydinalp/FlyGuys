using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameobjectExtensions {
    public static void SetLayer (this GameObject parent, int layer, bool includeChildren = true) {
        parent.layer = layer;
        if (includeChildren) {
            foreach (Transform trans in parent.transform.GetComponentsInChildren<Transform> (true)) {
                trans.gameObject.layer = layer;
            }
        }
    }
}
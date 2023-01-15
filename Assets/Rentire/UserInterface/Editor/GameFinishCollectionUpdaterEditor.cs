using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GameFinishCollectionUpdater))]
public class GameFinishCollectionUpdaterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameFinishCollectionUpdater gameFinishCollection = (GameFinishCollectionUpdater)target;
        if (GUILayout.Button("Collect"))
        {
            gameFinishCollection.UpdateCollection(1f, null);
        }
    }
}

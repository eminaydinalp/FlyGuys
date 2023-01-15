using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var gameManager = (GameManager)target;
        if(Application.isPlaying && GUILayout.Button("CHANGE STATE TO '" + gameManager.CurrentGameState + "'"))
        {
            gameManager.SetGameState(gameManager.CurrentGameState);
        }
    }
    
}

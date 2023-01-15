using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorManager))]
public class ColorManagerEditor : Editor
{
   public override void OnInspectorGUI()
   {
      DrawDefaultInspector();
      if (GUILayout.Button("REGENERATE LEVEL BASED COLOR SCRIPTABLE"))
      {
         (target as ColorManager).RegenerateLevelColors();
      }
   }
}

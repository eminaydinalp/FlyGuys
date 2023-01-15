using UnityEditor;
using UnityEngine;

public class ColorManagerWindow : EditorWindow
{

    [MenuItem("Rentire/Color Manager")]
    public void ShowWindow()
    {
        GetWindow(typeof(ColorManagerWindow));
    }

    private void OnGUI()
    {
        GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
    }
}

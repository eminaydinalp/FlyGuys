#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BaseTools : MonoBehaviour
{
    [MenuItem("Scenes/Game Runner")]
    static void GameRunner()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/_Game/_Scenes/Game_Runner.unity", OpenSceneMode.Single);
    }
    [MenuItem("Scenes/Game Base")]
    static void GameBase()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/_Game/_Scenes/Game_Base.unity", OpenSceneMode.Single);
    }

    [MenuItem("Scenes/Game Starter")]
    static void GameStarter()
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene("Assets/_Game/_Scenes/Game_Starter.unity", OpenSceneMode.Single);
    }


}
#endif
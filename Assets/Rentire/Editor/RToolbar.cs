using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rentire.Core;

public class RToolbar : MonoBehaviour
{
    [MenuItem("Rentire/Transform/Copy Transform Direction %1")]
    static void CopyTransformsDirection()
    {
        if (Selection.transforms.Length < 1)
            return;
        var selectedTransform = Selection.transforms[0];

        EditorGUIUtility.systemCopyBuffer = selectedTransform.forward.x + "," +
                                            selectedTransform.forward.y + "," +
                                            selectedTransform.forward.z;
    }

    [MenuItem("Rentire/Save Selection as Prefabs")]
    static void CreatePrefab()
    {
        GameObject[] objectArray = Selection.gameObjects;


        if (objectArray != null && objectArray.Length != 0)
        {
            string path = EditorUtility.SaveFolderPanel("Save prefabs to", "Assets/_GAME/Prefabs", "");

            foreach (GameObject gameObject in objectArray)
            {
                // Set the path as within the Assets folder,
                // and name it as the GameObject's name with the .Prefab format
                string localPath = path + "/" + gameObject.name + ".prefab";

                // Make sure the file name is unique, in case an existing Prefab has the same name.
                localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

                // Create the new Prefab.
                PrefabUtility.SaveAsPrefabAssetAndConnect(gameObject, localPath, InteractionMode.UserAction);
            }
        }
    }
    [MenuItem("Rentire/Save Level Prefab To Resources %L")]
    static void SaveLevelPrefab()
    {
        string assetPath = "Assets/__Data/Resources/";
        string levelPath = "Levels/";
        string oldLevelsPath = "Levels/Old/";
        var selectedObject = Selection.activeGameObject;
        if (!selectedObject)
        {
            Log.Warning("There was no object selected");
            return;
        }

        var existingObject = Resources.Load(levelPath + selectedObject.name);
        if (existingObject != null)
        {
            if (!AssetDatabase.IsValidFolder(assetPath + oldLevelsPath))
            {
                Log.Info("Folder not exists");
                AssetDatabase.CreateFolder(assetPath + levelPath, "Old");
            }

            AssetDatabase.MoveAsset(assetPath + levelPath + selectedObject.name + ".prefab", assetPath + oldLevelsPath + selectedObject.name + ".prefab");
            Log.Info("Old Prefab moved successfully");
        }

        PrefabUtility.SaveAsPrefabAssetAndConnect(selectedObject, "Assets/__Data/Resources/" + levelPath + selectedObject.name + ".prefab", InteractionMode.UserAction);

        Log.Info("Prefab saved successfully");
    }

    [MenuItem("Rentire/Reset Prefs")]
    static void ResetPrefs()
    {
        LocalPrefs.ClearAll();
        LocalPrefs.Save(LocalPrefs.defaultFileName, true);
    }

    [MenuItem("Rentire/Transform/Push World Transforms On Z Axis")]
    static void PushWorldTransformsZ()
    {
        var activeTransform = Selection.activeTransform;
        if (activeTransform == null)
            return;
        var result = EditorInputDialog.Show("Transform Pusher", "Pushes all child transforms in Z axis", "");

        if (float.TryParse(result, out float distance))
        {
            float initialDistance = 0f;
            for (int i = 0; i < activeTransform.childCount; i++)
            {
                var child = activeTransform.GetChild(i);
                child.transform.position = child.transform.position.AddZ(initialDistance);
                initialDistance += distance;
            }
        }
    }
    
    [MenuItem("Rentire/Transform/Push World Transforms On Y Axis")]
    static void PushWorldTransformsY()
    {
        var activeTransform = Selection.activeTransform;
        if (activeTransform == null)
            return;
        var result = EditorInputDialog.Show("Transform Pusher", "Pushes all child transforms in Y axis", "");

        if (float.TryParse(result, out float distance))
        {
            float initialDistance = 0f;
            for (int i = 0; i < activeTransform.childCount; i++)
            {
                var child = activeTransform.GetChild(i);
                child.transform.position = child.transform.position.AddY(initialDistance);
                initialDistance += distance;
            }
        }
    }
    
    [MenuItem("Rentire/Transform/Push Local Transforms On Y Axis")]
    static void PushLocalTransformsY()
    {
        var activeTransform = Selection.activeTransform;
        if (activeTransform == null)
            return;
        var result = EditorInputDialog.Show("Transform Pusher", "Pushes all child transforms in local Y axis", "");

        if (float.TryParse(result, out float distance))
        {
            float initialDistance = 0f;
            for (int i = 0; i < activeTransform.childCount; i++)
            {
                var child = activeTransform.GetChild(i);
                float a = 0;
                child.transform.localPosition = child.transform.localPosition.AddY(initialDistance);
                initialDistance += distance;
            }
        }
    }
    
    [MenuItem("Rentire/Transform/Push Local Transforms On Z Axis")]
    static void PushLocalTransformsZ()
    {
        var activeTransform = Selection.activeTransform;
        if (activeTransform == null)
            return;
        var result = EditorInputDialog.Show("Transform Pusher", "Pushes all child transforms in local Z axis", "");

        if (float.TryParse(result, out float distance))
        {
            float initialDistance = 0f;
            for (int i = 0; i < activeTransform.childCount; i++)
            {
                var child = activeTransform.GetChild(i);
                child.transform.localPosition = child.transform.localPosition.AddZ(initialDistance);
                initialDistance += distance;
            }
        }
    }
    

}

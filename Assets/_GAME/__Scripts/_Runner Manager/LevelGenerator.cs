using System.Collections.Generic;
using UnityEngine;
using Rentire.Core;
using System.Linq;
using Sirenix.OdinInspector;

public class LevelGenerator : RMonoBehaviour
{
    public List<Module> Modules;
    public List<LevelModules> LevelDesign;
    public string LevelPrefix = "Level";
    public int LevelNo = 1;
    public bool CreateSpline = true;

    [Button("GENERATE LEVEL")]
    public void GenerateLevel()
    {
        #region Destroy If Exists
        var go = GameObject.Find(LevelPrefix + LevelNo);
        var goModuleHelper = GameObject.Find("module_helper");
        if (go != null)
            DestroyImmediate(go);
        if (goModuleHelper != null)
            DestroyImmediate(goModuleHelper);
        #endregion


        // Create Level Parent Object
        var newLevel = new GameObject(LevelPrefix + LevelNo);
        newLevel.transform.ResetLocal();
        var levelScript = newLevel.AddComponent<Level>();

        // Create Level Builder Helper Transform
        var tempTransform = new GameObject("module_helper").transform;
        if (LevelDesign == null || LevelDesign.Count < 2)
            Log.Error("Check out the level design! - Level Design must have at least 2 modules");

        tempTransform.rotation = Quaternion.identity;
        tempTransform.position = Vector3.zero;

        if (CreateSpline)
        {
            // Create Spline

            var spline = new GameObject("Spline");
            spline.transform.SetParent(newLevel.transform);
            spline.transform.ResetLocal();
            var pathCreator = spline.AddComponent<PathCreation.PathCreator>();

            var transformList = new List<Transform>();
            var moduleList = new List<Module>();

            var trPath = Instantiate(tempTransform);
            trPath.position = tempTransform.position;
            trPath.rotation = tempTransform.rotation;
            transformList.Add(trPath);

            for (int i = 0; i < LevelDesign.Count; i++)
            {
                var moduleType = LevelDesign[i];

                var module = Modules.FirstOrDefault(x => x.ModuleType == moduleType);

                if (module == null)
                    Log.Error("Module '{0}' not found", moduleType.ToString());
#if UNITY_EDITOR

                var moduleGO = UnityEditor.PrefabUtility.InstantiatePrefab(module.ModulePrefab) as GameObject;

                moduleGO.transform.position = tempTransform.position;
                moduleGO.transform.rotation = tempTransform.rotation;

                tempTransform.localPosition = tempTransform.TransformPoint(module.ModuleSize);

                var newRotation = tempTransform.TransformDirection(module.ModuleDirection);
                tempTransform.rotation = Quaternion.LookRotation(newRotation, tempTransform.up);

                // Bezier
                trPath = Instantiate(tempTransform);
                trPath.position = tempTransform.position;
                trPath.rotation = tempTransform.rotation;
                transformList.Add(trPath);

                moduleGO.transform.SetParent(newLevel.transform);
                moduleList.Add(module);
#endif
            }

            var bezier = new PathCreation.BezierPath(transformList, false, PathCreation.PathSpace.xyz);
            bezier.ControlPointMode = PathCreation.BezierPath.ControlMode.Free;
            bezier.GlobalNormalsAngle = 90;

            int transformIndex = 0;
            for (int i = 0; i < bezier.NumSegments; i++)
            {
                int firstAnchorIndex = i * 3;

                var module = moduleList[transformIndex];

                Log.Info("Control point index {0} : {1}", firstAnchorIndex + 1, transformList[transformIndex].position + transformList[transformIndex].TransformDirection(Vector3.forward));

                bezier.SetPoint(firstAnchorIndex + 1, transformList[transformIndex].position + transformList[transformIndex].TransformDirection(Vector3.forward) * module.ModuleSize.z / 2) ;
                transformIndex++;

                Log.Info("Control point index {0} : {1}", firstAnchorIndex + 1, transformList[transformIndex].position + transformList[transformIndex].TransformDirection(Vector3.forward));
                bezier.SetPoint(firstAnchorIndex + 2, transformList[transformIndex].position + transformList[transformIndex].TransformDirection(Vector3.back) * module.ModuleSize.z / 2);
            }

            pathCreator.bezierPath = bezier;
            foreach (var item in transformList)
            {
                DestroyImmediate(item.gameObject);
            }
            // levelScript.PathCreator = pathCreator;
        }

        DestroyImmediate(tempTransform.gameObject);


        Log.Info("Level {0} Generated", LevelNo);
    }
}

public enum LevelModules
{
    Forward,
    ForwardShort,
    LeftTurn,
    RightTurn,
    UpTurn,
    DownTurn,
    ForwardUp,
    ForwardDown,
    UpToForward,
    DownToForward,
    Forward_RightLaneUp,
    RightTurn_RightLaneUp,
    RightTurn_MiddleLaneUp,
    RightTurn_LeftLaneUp,
    Forward_LeftLaneUp,
    LeftTurn_RightLaneUp,
    LeftTurn_MiddleLaneUp,
    LeftTurn_LeftLaneUp,
    Forward_MiddleLaneUp,
    Forward_RightLaneDown,
    Forward_LeftLaneDown,
    Forward_MiddleLaneDown,
    Forward_RightLane,
    Forward_LeftLane,
    Forward_MiddleLane,
}
[CreateAssetMenu(fileName = "Module", menuName = "Level Modules/Module")]
public class Module : ScriptableObject
{
    public GameObject ModulePrefab;
    public LevelModules ModuleType;
    public Vector3 ModuleSize;
    public Vector3 ModuleDirection;
#if UNITY_EDITOR
    [Button]
    public void RenameObject()
    {
        var path = UnityEditor.AssetDatabase.GetAssetPath(GetInstanceID());
        UnityEditor.AssetDatabase.RenameAsset(path, ModuleType.ToString());
    }

    [Button]
    public void FindObjectInPrefabs()
    {
        UnityEditor.EditorUtility.SetDirty(this);
        var path = "Assets/_GAME/Prefabs/Road Modules/v6/" + ModuleType.ToString() + ".prefab";
        var go = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        ModulePrefab = go;
    }

    [Button]
    public void MultiplySizesWith3()
    {
        UnityEditor.EditorUtility.SetDirty(this);
        ModuleSize = ModuleSize * 3f;
    }
#endif
}



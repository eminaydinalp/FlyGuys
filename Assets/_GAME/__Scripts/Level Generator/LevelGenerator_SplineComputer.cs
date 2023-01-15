using System;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using Rentire.Core;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LevelGenerator_SplineComputer : MonoBehaviour
{
    const string _levelPrefix = "Level";
    public GameObject baseRoadPrefab;
    public List<InterRoads> interRoadsList;
    public LayerMask groundLayer = 1 << 3;
    public Material mat;
    public int levelNo = 1;
    public float levelLength = 290f;
    public float maxXUnit = 10f;
    public float maxYUnit = 5f;
    public AnimationCurve heightCurve;
    public AnimationCurve topDownCurve;

    public GameObject finalPrefab;

    private SplineComputer _splineComputer;
    private float yOffset = 0f;

    [Button("Create Level")]
    [Tooltip("Creates level without considering offsets")]
    public void CreateSpline()
    {
        var levelGameObject = CreateGameObject(_levelPrefix + levelNo);
        var levelScript = levelGameObject.AddComponent<Level>();

        _splineComputer = AddSplineComputer(levelGameObject);
        CreateNodesAndSetPoints();
        AddSplineMesh(_splineComputer.gameObject);
        AddFinishPrefab(levelGameObject);

        levelScript.splineComputer = _splineComputer;
    }

    [Button]
    [Tooltip("Creates level with offsets, must be filled from 0-1 completely")]
    public void CreateSplineWithOffsetY()
    {
        var levelGameObject = CreateGameObject(_levelPrefix + levelNo);
        var levelScript = levelGameObject.AddComponent<Level>();
        _splineComputer = AddSplineComputer(levelGameObject);
        CreateNodesAndSetPoints();
        AddSplineMesh(_splineComputer.gameObject, true);
        AddFinishPrefab(levelGameObject);
    }

    [Button]
    public void FillInTheBlanks()
    {
        var list = new List<MissingBlanks>();
        for (int i = 0; i < interRoadsList.Count; i++)
        {
            var currentRoad = interRoadsList[i];
            if (i == 0)
            {
                if(currentRoad.start > 0f)
                    list.Add(new MissingBlanks {start = 0f, end = currentRoad.start});
            }
            else if(i <  interRoadsList.Count - 1)
            {
                var prevRoad = interRoadsList[i - 1];
                var nextRoad = interRoadsList[i + 1];
                if(prevRoad.end < currentRoad.start)
                    list.Add(new MissingBlanks {start =prevRoad.end, end = currentRoad.start});
                if(currentRoad.end < nextRoad.start)
                    list.Add(new MissingBlanks {start =currentRoad.end, end = nextRoad.start});
            }
            if (i == interRoadsList.Count - 1)
            {
                var prevRoad = interRoadsList[i - 1];
                
                if(prevRoad.end < currentRoad.start)
                    list.Add(new MissingBlanks {start =prevRoad.end, end = currentRoad.start});
                
                if(currentRoad.end < 1f)
                    list.Add(new MissingBlanks {start =currentRoad.end, end = 1f});
            }
        }

        for (int i = 0; i < list.Count; i++)
        {
            interRoadsList.Add(new InterRoads
            {
                start = list[i].start,
                end = list[i].end,
                continueWithY = false,
                meshObject = baseRoadPrefab,
                xOffset = 0f,
                yOffset = 0f
            });
        }

        interRoadsList = interRoadsList.OrderBy(x => x.start).ToList();
    }

    struct MissingBlanks
    {
        public float start;
        public float end;
    }
    
    GameObject CreateGameObject(string goName, Transform parent = null)
    {
        var existingObject = GameObject.Find(goName);
        if(existingObject)
            DestroyImmediate(existingObject);
            
        var go = new GameObject(goName);
        go.transform.ResetLocal();
        go.transform.ResetScale();
        go.transform.SetParent(parent);
        return go;
    }

    SplineComputer AddSplineComputer(GameObject levelGO)
    {
        var go = CreateGameObject("SplineComputer", levelGO.transform);
        var splineComp = go.AddComponent<SplineComputer>();
        splineComp.sampleMode = SplineComputer.SampleMode.Uniform;
        return splineComp;
    }

    void AddSplineMesh(GameObject splineGo, bool withOffsets = false)
    {
        var meshGo = CreateGameObject("SplineMesh", splineGo.transform);
        var splineMesh = meshGo.AddComponent<SplineMesh>();
        splineMesh.spline = _splineComputer;
        splineMesh.buildOnEnable = true;
        var o = splineMesh.gameObject;
        o.layer = 3;
        o.isStatic = true;
        splineMesh.AddComponent<MeshCollider>();
        splineMesh.RemoveChannel(0);
        splineMesh.meshIndexFormat = IndexFormat.UInt32;
        if (!withOffsets)
            CreateChannels(splineMesh);
        else
            CreateChannelsWithOffset(splineMesh);
        splineMesh.GetComponent<MeshRenderer>().material = mat;
        splineMesh.RebuildImmediate();
    }

    void CreateChannels(SplineMesh splineMesh)
    {
         int totalRoadList = 0;
        
        if(interRoadsList.Count > 0)
        {
            for (int i = 0; i < interRoadsList.Count; i++)
            {
                var interRoad = interRoadsList[i];
                if (i == 0)
                {
                    if(interRoad.start > 0)
                    {
                        Log.Info("Start has base & channel");
                        totalRoadList += 2;
                        AddChannel(splineMesh, baseRoadPrefab, 0f, interRoad.start, "ChannelBase");
                        AddChannel(splineMesh, interRoad.meshObject, interRoad.start, interRoad.end, "ChannelInter");
                    }
                    else
                    {
                        totalRoadList++;
                        AddChannel(splineMesh, interRoad.meshObject, interRoad.start, interRoad.end, "ChannelInter");
                    }

                    if (interRoadsList.Count == 1 && interRoad.end < 1f)
                    {
                        Log.Info("Total Road list is '0'");
                        totalRoadList++;
                        AddChannel(splineMesh, baseRoadPrefab, interRoad.end, 1f, "ChannelBase");
                    }
                }
                else if (i == interRoadsList.Count - 1 )
                {
                    if( interRoad.end < 1 && interRoad.start > interRoadsList[i - 1].end){
                        Log.Info("Last has 3");
                        totalRoadList += 3;
                        AddChannel(splineMesh, baseRoadPrefab, interRoadsList[i - 1].end, interRoad.start, "ChannelBase");
                        AddChannel(splineMesh, interRoad.meshObject, interRoad.start, interRoad.end, "ChannelInter");
                        AddChannel(splineMesh, baseRoadPrefab, interRoad.end, 1, "ChannelBase", interRoad.yOffset);
                    }
                    else
                    {
                        totalRoadList += 2;
                        AddChannel(splineMesh, baseRoadPrefab, interRoadsList[i - 1].end, interRoad.start, "ChannelBase");
                        AddChannel(splineMesh, interRoad.meshObject, interRoad.start, interRoad.end, "ChannelInter");
                    }
                }
                else
                {
                    if (interRoad.start > interRoadsList[i - 1].end)
                    {
                        Log.Info("Inter has 2");
                        totalRoadList += 2;
                        AddChannel(splineMesh, baseRoadPrefab, interRoadsList[i - 1].end, interRoad.start, "ChannelBase");
                        AddChannel(splineMesh, interRoad.meshObject, interRoad.start, interRoad.end, "ChannelInter");
                    }
                    else
                    {
                        Log.Info("Inter has 1");
                        totalRoadList++;
                        AddChannel(splineMesh, interRoad.meshObject, interRoad.start, interRoad.end, "ChannelInter");
                    }    
                }
            }
        }
        else
        {
            AddChannel(splineMesh, baseRoadPrefab, 0, 1f, "ChannelBase");
        }
    }

    void CreateChannelsWithOffset(SplineMesh splineMesh)
    {
        int totalRoadList = 0;

        yOffset = 0f;
        float xOffset = 0f;
        
        if(interRoadsList.Count > 0)
        {
            var interRoadStart = interRoadsList[0];
            // var channel = AddChannel(splineMesh, interRoadStart.meshObject, interRoadStart.start, interRoadStart.end, "Channel 1", yOffset + interRoadStart.yOffset);
            for (int i = 0; i < interRoadsList.Count; i++)
            {
                var interRoad = interRoadsList[i];
                
                // AddMesh(channel, );
                
                AddChannel(splineMesh, interRoad.meshObject, interRoad.start, interRoad.end, "Channel " + (i+1), yOffset + interRoad.yOffset, interRoad.extend);
                
                if (interRoad.continueWithY)
                    yOffset += interRoad.yOffset;
            }
        }
        else
        {
            AddChannel(splineMesh, baseRoadPrefab, 0, 1f, "ChannelBase");
        }
    }
    
    SplineMesh.Channel AddChannel(SplineMesh splineMesh,GameObject prefab, float from, float to, string channelName, float offsetY = 0f, bool extend = false)
    {
        var mesh = Instantiate(prefab);
        Log.Info("Channel creating : "+ channelName);
        var channel = splineMesh.AddChannel(mesh.GetComponent<MeshFilter>().sharedMesh,  string.IsNullOrEmpty(channelName) ? "Channel" : channelName);
        channel.clipFrom = from;
        channel.clipTo = to;
        channel.GetMesh(0).offset = Vector3.up * offsetY;
        channel.count = 1;
        channel.autoCount = !extend;
        DestroyImmediate(mesh.gameObject);
        return channel;
    }

    void AddMesh(SplineMesh.Channel channel, GameObject prefab, float from, float to, float offsetY = 0f, float offsetX = 0f)
    {
        var meshGO = Instantiate(prefab);
        channel.AddMesh(meshGO.GetComponent<MeshFilter>().sharedMesh);
        var meshCount = channel.GetMeshCount();
        var mesh = channel.GetMesh(meshCount - 1);
        mesh.offset = mesh.offset.WithXY(offsetX, offsetY);
    }

    void AddFinishPrefab(GameObject levelGo)
    {
#if UNITY_EDITOR
        var finish = PrefabUtility.InstantiatePrefab(finalPrefab) as GameObject;
        if (finish != null)
        {
            finish.transform.SetParent(levelGo.transform);
            var finalPoint = _splineComputer.Evaluate(1D);

            finish.transform.position = finalPoint.position.AddY(yOffset);
            finish.transform.rotation = finalPoint.rotation;
        }

#endif
    }
    

    [Button("Create Nodes")]
    void CreateNodesAndSetPoints(bool withOffsets = false)
    {
        SplinePoint[] splinePoints = new SplinePoint[topDownCurve.keys.Length];

        for (int i = 0; i < topDownCurve.keys.Length; i++)
        {
            var key = topDownCurve.keys[i];
            Log.Info("Key Time : " + key.time);

            splinePoints[i].normal = Vector3.up;
            var position =
                i == 0
                    ? Vector3.zero
                    : Vector3.forward * topDownCurve.keys[i].time * levelLength
                      + Vector3.right * (maxXUnit * (key.value - topDownCurve.keys[i - 1].value))
                      + Vector3.up * (maxYUnit * (heightCurve.Evaluate(key.time) - 1f));
            splinePoints[i].SetPosition(position);
            splinePoints[i].size = 1f;
            Log.Info("Calculated position : " + splinePoints[i].position);
            if (i != 0)
            {
                Log.Info("Height Value : " + heightCurve[i].value);
                Log.Info("Curve Value : " + key.value);
                Log.Info("Right/Left Value : " + (key.value - topDownCurve.keys[i - 1].value));
                Log.Info("Right/Left Estimation : " +
                         Vector3.right * (maxXUnit * (key.value - topDownCurve.keys[i - 1].value)));
                Log.Info("Up Value : " + (heightCurve.Evaluate(key.time) - 1f));
                Log.Info("Up Value Estimation : " +
                         Vector3.up * (maxYUnit * (heightCurve[i].value - heightCurve.keys[i - 1].value)));
            }
        }

        _splineComputer.SetPoints(splinePoints);

        Log.Info("NODES : " + _splineComputer.GetNodes().Count);
    }

    private void OnValidate()
    {
        return;
        Log.Info("Validating");
        if (interRoadsList.Count > 0)
        {
            interRoadsList = interRoadsList.OrderBy(x => x.start).ToList();
            for (int i = 0; i < interRoadsList.Count - 1; i++)
            {
                
                var road = interRoadsList[i];
                var nextRoad = interRoadsList[i + 1];

                //Check sameness first
                if (road.start > road.end || Math.Abs(road.start - road.end) < 0.001f)
                {
                    Log.Info("Below Tolerance");
                    
                    if (Math.Abs(road.start - 1f) < 0.001f)
                    {
                        road.start -= 0.1f;
                    }
                    var end = road.start + 0.1f;
                    
                    if (end >= 1f)
                    {
                        end = 1f;
                    }
                        
                    interRoadsList[i] = GetInterRoad(road.start, end,
                        road.meshObject);
                 
                }

                if (nextRoad.start < road.end)
                {
                    interRoadsList[i + 1] = GetInterRoad(road.end, nextRoad.end,
                        nextRoad.meshObject);
                }

                if (nextRoad.start > nextRoad.end || Math.Abs(nextRoad.start - nextRoad.end) < 0.001f)
                {
                    Log.Info("Below Tolerance");
                    interRoadsList[i + 1] = GetInterRoad(nextRoad.start, nextRoad.start + 0.1f,
                        nextRoad.meshObject);
                }
            }
        }
    }

    InterRoads GetInterRoad(float start, float end, GameObject mesh, float yOffset = 0f)
    {
        return new InterRoads()
        {
            start = start,
            end = end,
            meshObject = mesh,
            yOffset = 0f
        };
    }
}

[Serializable]
public class InterRoads
{
    public GameObject meshObject;
    [Range(0,1f)]
    public float start;
    [Range(0,1f)]
    public float end;
    
    [Tooltip("Starts the road with the offset y")]
    public float yOffset;
    public float xOffset;
    [Tooltip("Continues the road with the y Offset of previous one")]
    public bool continueWithY;
    [Tooltip("Extends the road part on spline(Count = 1)")]
    public bool extend;
}

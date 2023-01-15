using System.Collections.Generic;
using System.Linq;
using Rentire.Core;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ColorManager : Singleton<ColorManager>
{
    [Header("Color Palettes")]
    public LevelColorsScriptable LevelColors;

    
    //[InfoBox("Eger 'true' ise level üzerindeki No'yu alir")]
    [BoxGroup("GroupBox")]
    public bool useColorsOnLevelScript;
    //[InfoBox("Eger 'true' ise level 'overridedColorNo' ne ise onu alir")]
    [BoxGroup("GroupBox")]
    public bool overrideColor = false;
    [BoxGroup("GroupBox")]
    public int overridedColorNo;
    [BoxGroup("GroupBox")]
    public int ChoosenColorNo;
    //[InfoBox("Kac level'da bir renk degisecek")]
    [BoxGroup("GroupBox")]
    public int levelCountForLevelChange;

    public Camera mainCamera;

    public List<Material> Materials;
    
    public Material Mat_Player;
    public Material Mat_Enemy;
    public Material Mat_Ground;
    public Material Mat_Wall;
    
    private void Awake()
    {
        if (!mainCamera)
            mainCamera = Camera.main;

        if (overrideColor)
            ChoosenColorNo = overridedColorNo;
        
        SetFields();
    }

    private void Start()
    {
        SetLevelColors(LevelColors, overrideColor ? overridedColorNo : -1);
    }

    public void SetLevelColors(LevelColorsScriptable levelcolors, int choosenColorNo = -1)
    {
        if (choosenColorNo == -1)
        {
            int selected = Mathf.FloorToInt((float)LevelManager.Instance.CurrentLevelNo / levelCountForLevelChange);
            choosenColorNo = selected % levelcolors.Levels.Count;
        }
        
        if (choosenColorNo >= levelcolors.Levels.Count)
        {
            choosenColorNo %= levelcolors.Levels.Count;
        }
        
        LevelColors = levelcolors;

        var colorSet = levelcolors.Levels[choosenColorNo];

        mainCamera.backgroundColor = colorSet.ColorSky;
        RenderSettings.fogColor = colorSet.ColorFog;
        
        if(colorSet.Skybox != null)
            RenderSettings.skybox = colorSet.Skybox;

        for (int i = 0; i < Materials.Count; i++)
        {
            var mat = Materials[i];
            var colorSetMat = colorSet.Materials.FirstOrDefault(x => x.Name.Equals(mat.name));
            
            if(colorSetMat != null)
                mat.SetColor("_BaseColor", colorSetMat.Color);
        }

        ChoosenColorNo = choosenColorNo;
    }

    void SetFields()
    {
        ReflectionHelpers<Material>.CastFieldsIntoList(ref Materials, Instance);

        Materials.RemoveAll(x => x == null);
    }

    [Button("Regenerate Colors")]
    public void RegenerateLevelColors()
    {
        Validate();
    }


    private void Validate()
    {
        SetFields();

#if UNITY_EDITOR
        EditorUtility.SetDirty(LevelColors);
        for (int k = 0; k < LevelColors.Levels.Count; k++)
        {
            var level = LevelColors.Levels[k];
            
            level.Materials ??= new List<MaterialProperties>();
        
            for (int i = 0; i < Materials.Count; i++)
            {
                var mat = Materials[i];
                var matProps = new MaterialProperties();

                if(mat == null)
                    continue;
            
                matProps.Name = mat.name;
                //matProps.Color = Color.white;
            
                if (!level.Materials.Any(x=> x.Name.Equals(matProps.Name)))
                {
                    level.Materials.Add(matProps);
                }
            }
            
            // Material içinde olmayanları çıkar
            level.Materials.RemoveAll(x => !Materials.Any(m => m.name.Equals(x.Name)));
        }
#endif
        
    }
}

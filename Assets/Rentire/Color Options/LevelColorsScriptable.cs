using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Colors Option", menuName = "Color Options/Level Based Colors", order = 2)]
public class LevelColorsScriptable : ScriptableObject
{
    public List<LevelOrder> Levels;
}

[System.Serializable]
public class MaterialProperties
{
    public string Name;
    public Color Color;
}

[System.Serializable]
public class LevelOrder
{
    public List<MaterialProperties> Materials;
    public Color ColorSky;
    public Color ColorVolumetricFog;
    public Color ColorFog;
    public Material Skybox;
}

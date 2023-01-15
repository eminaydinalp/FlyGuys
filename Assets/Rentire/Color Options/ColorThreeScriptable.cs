using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Three Option", menuName = "Color Options/3 Color Variant", order = 1)]
public class ColorThreeScriptable : ScriptableObject
{
    public List<ThreeColor> Colors;
}

[System.Serializable]
public class ThreeColor
{
    public ThreeColor()
    {
        Color1 = Color.white;
        Color2 = Color.white;
        Color3 = Color.white;
    }
    public Color Color1;
    public Color Color2;
    public Color Color3;
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Color One Option", menuName = "Color Options/1 Color Variant", order = 3)]


public class ColorOneScriptable : ScriptableObject
{
    public List<OneColor> Colors;
}

[System.Serializable]
public class OneColor
{
    public OneColor()
    {
        ColorTurkuaz = Color.white;
        ColorYesil = Color.white;
        ColorSari = Color.white;
        ColorTuruncu = Color.white;
        ColorKirmizi = Color.white;
        ColorPembe = Color.white;
        ColorMor = Color.white;
        ColorMavi = Color.white;
    
    }
    public Color ColorTurkuaz;
    public Color ColorYesil;
    public Color ColorSari;
    public Color ColorTuruncu;
    public Color ColorKirmizi;
    public Color ColorPembe;
    public Color ColorMor;
    public Color ColorMavi;
}
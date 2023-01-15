using Rentire.Core;
using UnityEngine;

namespace _GAME.__Scripts.Controller
{
    public class ColorController : Singleton<ColorController>
    {
        public Color[] colors;
        public Color oneColor;
        public Color twoColor;
        
        public Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        void Start()
        {
            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = oneColor;
            colorKey[0].time = 0.0f;
            colorKey[1].color = twoColor;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);

            // What's the color at the relative time 0.25 (25 %) ?
            //Debug.Log(gradient.Evaluate(0.25f));
        }
    }
}
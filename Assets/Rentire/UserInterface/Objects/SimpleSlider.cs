using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleSlider : MonoBehaviour
{
    [Header("Vibration")]
    public bool IncludeVibration = false;

    [Header("Percentage")]
    public bool ShowPercentage = false;
    public TextMeshProUGUI TextPercentage;
    
    public float value { get {
            return _value;
        }
        set
        {
            _value = value;
            UpdateValue();
        }
    }
    private float _value;

    public Image FillImage;


    private void UpdateValue()
    {
        FillImage.fillAmount = Mathf.Clamp01(_value);

        if(ShowPercentage && TextPercentage != null)
        {
            int finalPercentage = Mathf.Clamp(Mathf.FloorToInt(FillImage.fillAmount * 100), 0, 100);
            TextPercentage.text = finalPercentage + "%";

#if MOREMOUNTAINS_NICEVIBRATIONS
            if(IncludeVibration)
            {
                MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.RigidImpact);
            }
#endif
        }
    }
}

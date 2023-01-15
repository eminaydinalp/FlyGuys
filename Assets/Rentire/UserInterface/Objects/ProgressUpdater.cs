using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using TMPro;
using UnityEngine;

public class ProgressUpdater : Singleton<ProgressUpdater>
{
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private SimpleSlider Slider;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        // Check Objects if assigned
        CheckIfObjectAssigned(LevelText);
        CheckIfObjectAssigned(Slider);
    }

    public void UpdateSlider(float currentPosition, float startPosition, float endPosition)
    {
        if (currentPosition < startPosition)
            currentPosition = startPosition;

        var progress = 1f - Mathf.Clamp01((endPosition - currentPosition) / (endPosition - startPosition));
        UpdateSlider(progress);
    }

    public void UpdateSlider(float progressValue)
    {
        Slider.value = Mathf.Clamp01(progressValue);
    }

    public void UpdateLevelText(int levelNo, bool isBonus = false)
    {
        LevelText.text = "Level " + levelNo;

        if(isBonus)
            LevelText.text = "BONUS";
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    private TMP_Text levelText;
    void Start()
    {
        levelText = GetComponent<TMP_Text>();
        if (levelText != null)
            levelText.text = "LEVEL " + LevelManager.Instance.CurrentLevelNo;
    }

}

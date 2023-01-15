using System;
using System.Collections;
using System.Collections.Generic;
using Rentire.Core;
using Rentire.Utils;
using TMPro;
using UnityEngine;

public class CoinText : RMonoBehaviour
{
    private TMP_Text coinText;

    private void Awake()
    {
        if (eventManager != null)
        {
            eventManager.event_CollectionUpdated += OnCoinUpdated;
        }
        coinText = GetComponent<TMP_Text>();
        OnCoinUpdated();
    }


    private void OnCoinUpdated()
    {
        coinText.text = UserPrefs.GetTotalCollection().ToString();
    }
    

}

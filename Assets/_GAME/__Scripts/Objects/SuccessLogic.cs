using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MEC;
using TMPro;
using UnityEngine;

public class SuccessLogic : RMonoBehaviour,IGameStateObserver
{
    public TMP_Text text_EarnedCoin;
    public RectTransform go_NextButton;
    public RectTransform go_CoinTransform;
    private int _earnedCoin => CollectionUpdater.Instance.earnedCoin;

    private void Awake()
    {
        AddToGameObserverList();
    }

    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }
    

    public void OnGameStateChanged()
    {
        if (gameState == GameState.Success)
        {
            ReloadEarnedCoin();
        }
    }

    void ReloadEarnedCoin()
    {
        text_EarnedCoin.text = _earnedCoin.ToString();

    }

    public void Button_NextLevel()
    {
        
        var coinAmount = _earnedCoin / 10;
        var remainCoin = _earnedCoin % 10;
        Timing.RunCoroutine(GiveCoin(remainCoin, coinAmount));
    }

    IEnumerator<float> GiveCoin(int remainCoin,int coinAmount)
    {
        go_CoinTransform.DOScale(new Vector3(0, 1, 1), 0.2f).OnComplete(()=>go_CoinTransform.gameObject.SetActive(false));
        go_NextButton.gameObject.SetActive(false);
        yield return Timing.WaitForSeconds(0.1f);
        for (int i = 0; i < 10; i++)
        {
            CollectionUpdater.Instance.CollectWithAnimation(text_EarnedCoin.rectTransform,coinAmount,true,true);
        }
        CollectionUpdater.Instance.CollectWithAnimation(text_EarnedCoin.rectTransform,remainCoin,true,true);
        yield return Timing.WaitForSeconds(1f);
        UIManager.Instance.Button_NextLevel();
    }
 
    private void OnEnable()
    {
        ReloadEarnedCoin();
    }
}

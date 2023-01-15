using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
#if DOTWEEN_API
using DG.Tweening;
#endif
using Lean.Pool;
using Rentire.Core;
using TMPro;
using UnityEngine;

public class GameFinishCollectionUpdater : Singleton<GameFinishCollectionUpdater>
{
    public bool IsVibrationEnabled;
    public GameObject UIObjectPrefab;
    [SerializeField] RectTransform startPositionFrom;
    [SerializeField] RectTransform finalPositionToGo;
    [SerializeField] RectTransform[] rectTransforms;
    [SerializeField] TextMeshProUGUI textCoin;

    int currentAmount;
    int amount;

    public void UpdateCollectionText(int amount)
    {
        textCoin.text = "+" + amount.ToString();
        this.amount = amount;
        this.currentAmount = amount;
    }

    public void UpdateCollection(float delay, Action<int> OnComplete)
    {
        foreach (var item in rectTransforms.ToList())
        {
            var rect = LeanPool.Spawn(UIObjectPrefab, transform, false).GetComponent<RectTransform>();
            rect.anchoredPosition = item.anchoredPosition;
            rect.localPosition = item.localPosition;
            rect.anchorMin = item.anchorMin;
            rect.anchorMax = item.anchorMax;

            rect.SetParent(startPositionFrom);
            rect.anchoredPosition = Vector3.zero;

            rect.SetParent(item);
#if DOTWEEN_API
            rect.DOAnchorPos(Vector3.zero, 0.5f).SetDelay(.5f).OnComplete(()=> {
                rect.SetParent(finalPositionToGo);
                rect.DOAnchorPos(Vector3.zero, 0.7f).SetDelay(delay);
            });
#endif

        }
#if DOTWEEN_API
        DOTween.To(() => currentAmount, x => currentAmount = x, 0, 1f).SetDelay(.5f).OnUpdate(()=> {
            textCoin.text = currentAmount.ToString();
            if(IsVibrationEnabled)
            {
#if MOREMOUNTAINS_NICEVIBRATIONS
                MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.RigidImpact);
#endif
            }
        });
#endif

        CallMethodWithDelay(() => {
            if (OnComplete != null)
                OnComplete.Invoke(amount);
        }, .5f + delay + 0.7f);
    }
}

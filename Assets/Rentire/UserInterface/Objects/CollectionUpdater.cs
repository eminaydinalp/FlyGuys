#if DOTWEEN_API
using DG.Tweening;
#endif
using Lean.Pool;
using Rentire.Core;
using Rentire.Utils;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;


public class CollectionUpdater : Singleton<CollectionUpdater>
{
    [Header("Vibration")] [Tooltip("Needs MM Nice Vibrations")]
    public bool IncludeVibration = false;

    [Header("Screen Animation")] public RectTransform CollectibleImageTransform;
    public GameObject CollectionItemPrefab;
    public float AnimationDuration;

    [Header("Text Update")] public TMP_Text CoinText;
    public float PunchAnimationAmount = 0.1f;

    private int totalAmount = 0;
#if DOTWEEN_API
    private Tweener punchTweener;
#endif

    public int earnedCoin;

    public Ease scaleEase;
    public bool startWithScale = true;
    public float scaleDuration = 0.15f;
    public Ease animationEase;

    private void Start()
    {
        totalAmount = UserPrefs.GetTotalCollection();
        if (CoinText)
            CoinText.text = totalAmount.ToString();
    }

    public void Collect(int amount = 1)
    {
        CollectionComplete(amount);
    }


    /// <summary>
    /// To use this, Cam Manager should have a Canvas named MasterCanvas, and UI Camera must be same size with Main Camera and must be the child of
    /// Main Camera
    /// </summary>
    /// <param name="collectiblePosition"></param>
    /// <param name="amount"></param>
    [Button("Collect With Animation")]
    public void CollectWithAnimation(Vector3 collectiblePosition, int amount = 1, bool randomizePosition = false)
    {
        if (randomizePosition)
        {
            collectiblePosition =
                collectiblePosition.AddY(Random.Range(-0.25f, 0.25f)).AddX(Random.Range(-0.25f, 0.25f));
        }

        RectTransform clone = PoolManager.Instance.Spawn_Object(PoolsEnum.CoinUI, CollectibleImageTransform.transform.parent, false)
            .GetComponent<RectTransform>();
        var point = RectExtensions.WorldPointToCanvasLocalRectTransformPoint(collectiblePosition, Camera.main,
            CamManager.Instance.MasterCanvas,
            CollectibleImageTransform);
        clone.localPosition = point;
        clone.localScale = Vector3.zero;

#if MOREMOUNTAINS_NICEVIBRATIONS
        if (IncludeVibration)
        {
            MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes
                .RigidImpact);
        }
#endif

        if (startWithScale)
        {
            clone.DOScale(Vector3.one, scaleDuration)
                .SetEase(scaleEase)
                .OnComplete(() =>
                    clone.DOAnchorPos(CollectibleImageTransform.anchoredPosition, AnimationDuration)
                        .SetEase(animationEase).OnComplete(() =>
                        {
                            CollectionComplete(amount);
                            LeanPool.Despawn(clone.gameObject);
                        }));
        }
        else
        {
            clone.DOAnchorPos(Vector3.zero, AnimationDuration).SetEase(animationEase).OnComplete(() =>
            {
                CollectionComplete(amount);
                LeanPool.Despawn(clone.gameObject);
            });
        }
    }

    [Button("Collect With Animation")]
    public void CollectWithAnimation(RectTransform rect, int amount = 1, bool randomizePosition = false, bool withRandomDelay = false, bool withScale = false)
    {
        var go = new GameObject("Temp");
        go.transform.SetParent(CamManager.Instance.MasterCanvas.transform);
        go.transform.ResetPosition();
        var tempTransform = go.AddComponent<RectTransform>();
        tempTransform.SetPivotAndAnchors(new Vector2(0, 1));
        tempTransform.SetLeftTopPosition(Vector2.zero);
        tempTransform.ResetScale();
        
        var point = RectExtensions.WorldPointToCanvasLocalRectTransformPoint(rect.transform.position, CamManager.Instance.UICamera,
            CamManager.Instance.MasterCanvas,
            tempTransform);
        
        if (randomizePosition)
        {
            point =
                point.AddY(Random.Range(-50f, 50f)).AddX(Random.Range(-50f, 50f));
        }

        RectTransform clone = PoolManager.Instance.Spawn_Object(PoolsEnum.CoinUI, CamManager.Instance.MasterCanvas.transform)
            .GetComponent<RectTransform>();
        
        clone.localPosition = point;
        clone.SetParent(CollectibleImageTransform);

#if MOREMOUNTAINS_NICEVIBRATIONS
        if (IncludeVibration)
        {
            MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes
                .RigidImpact);
        }
#endif
        float delay = withRandomDelay ? Random.Range(0,0.25f) : 0f;
        
        if (withScale)
        {
            clone.DOScale(Vector3.one, scaleDuration)
                .SetDelay(delay)
                .SetEase(scaleEase)
                .OnComplete(() =>
                    clone.DOAnchorPos(Vector3.zero, AnimationDuration)
                        .SetEase(animationEase).OnComplete(() =>
                        {
                            CollectionComplete(amount);
                            LeanPool.Despawn(clone.gameObject);
                        }));
        }
        else
        {
            clone.DOAnchorPos(Vector3.zero, AnimationDuration)
                .SetDelay(delay)
                .SetEase(animationEase).OnComplete(() =>
            {
                CollectionComplete(amount);
                LeanPool.Despawn(clone.gameObject);
            });
        }
        
        Destroy(tempTransform.gameObject);
    }
    
    void CollectionComplete(int amount)
    {
#if DOTWEEN_API
        if (punchTweener == null || !punchTweener.IsPlaying())
            punchTweener = CollectibleImageTransform.DOPunchScale(Vector3.one * PunchAnimationAmount, 0.15f);
#endif
        totalAmount += amount;
        earnedCoin += amount;
        // if (CoinText)
        //     CoinText.text = totalAmount.ToString();
        UserPrefs.IncreaseCollection(amount);
        // EventManager.Instance.Invoke_CollectionUpdated();

    }
}
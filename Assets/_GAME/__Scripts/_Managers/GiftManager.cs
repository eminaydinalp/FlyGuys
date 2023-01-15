using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.NiceVibrations;
using Newtonsoft.Json;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;
using UnityEngine.UI;

public class GiftManager : Singleton<GiftManager>
{
    public bool IsGiftManagerActive;
    public bool IncludeVibration = true;
    public string Gifts_Resource_Path = "Gift_Sprites";
    
    public GameObject FillParent;
    public GameObject GiftToUnlock;
    public Image GiftImage;
    public Text Text_Percentage;
    public GiftList AllGifts;
    public Image fillImage;
    public float GiftFillDuration = 0.5f;
    
    private int currentGiftNo;
    private float currentGiftIncreaseAmount;
    private Gift currentGift;

    // Start is called before the first frame update
    void Start()
    {
        CalculateCurrentGift();
        
        fillImage.fillAmount = UserPrefs.GetGiftSliderPercentage();
        int currentPercentage = (int)(fillImage.fillAmount * 100f);
        Text_Percentage.text = currentPercentage + "%";
        GiftToUnlock.SetActive(false);
    }

    public bool IsGiftAvailable()
    {
        return currentGift != null;
    }
    
    public void IncreaseGiftPercentage()
    {
        int frame = 0;
        
        float fillAmount = UserPrefs.UpdateGiftSliderPercentage(currentGiftIncreaseAmount);
        
        fillImage.DOFillAmount(fillAmount, GiftFillDuration).OnUpdate(() =>
        {
            frame++;
            int currentPercentage = Mathf.Clamp ((int)(fillImage.fillAmount * 100f), 0, 100);
            Text_Percentage.text = currentPercentage + "%";
            if(IncludeVibration && frame % 5 == 0)
            {
                FeedbackManager.Instance.Vibrate(HapticTypes.SoftImpact);
            }
        }).OnComplete(() =>
        {
            if (fillAmount >= 0.99f)
            {
                CallMethodWithDelay(ShowGift, 0.1f);
            }
            else
            {
                CallMethodWithDelay(()=> ViewManager.Instance.Hide_View_Gift(), 0.1f);
            }
            
        });

    }

    void ShowGift()
    {
        FillParent.SetActive(false);
        GiftImage.sprite = AllGifts.Gifts[currentGiftNo].GiftImage;
        GiftToUnlock.SetActive(true);
    }

    //Button assigned method - Unity Inspector
    public void TakeGift()
    {
        UserPrefs.AddToGifts(currentGift);
        ViewManager.Instance.Hide_View_Gift();
    }
    
    //Button assigned method - Unity Inspector
    public void NoThanks()
    {
        UserPrefs.IncreaseGift();
        ViewManager.Instance.Hide_View_Gift();
    }

    void CalculateCurrentGift()
    {
        currentGiftNo = UserPrefs.GetGift();
        var myEarnedGifts = UserPrefs.GetEarnedGifts();
        var giftsToUnlock = AllGifts.Gifts.Where(x=> myEarnedGifts.All(y => y.SpriteName != x.SpriteName)).ToList();
     
        if (giftsToUnlock.Count > 0 && currentGiftNo >= giftsToUnlock.Count)
        {
            currentGiftNo = currentGiftNo % giftsToUnlock.Count;
        }
        if(giftsToUnlock.Count > 0)
        {
            currentGift = giftsToUnlock[currentGiftNo];
            currentGiftIncreaseAmount = currentGift.GiftIncreaseAmount;
            currentGift.GiftImage = LoadObject<Sprite>(Gifts_Resource_Path + "/" + currentGift.SpriteName);
        }
        else
        {
            currentGift = null;
        }
    }

    private void OnValidate()
    {
        if (AllGifts != null && AllGifts.Gifts != null)
        {
            for (int i = 0; i < AllGifts.Gifts.Count; i++)
            {
                var gift = AllGifts.Gifts[i];
                gift.SpriteName = gift.GiftImage.name;
            }
        }
    }
}
[Serializable]
public class Gift
{
    [JsonIgnore]
    public Sprite GiftImage;
    public string SpriteName;
    public float GiftIncreaseAmount;
}
[Serializable]
public class GiftList
{
    public List<Gift> Gifts;
}

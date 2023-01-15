using System.Collections.Generic;
using Newtonsoft.Json;
using Rentire.Core;
using UnityEngine;

namespace Rentire.Utils
{
    public enum PrefType {
        Sound,
        Vibration,
        AdConsent,
        LevelNo,
        PlayerCharacter,
        PlayerOwnedCharacters,
        UnlockedCharacters,
        NoAds_AdsRemoved,
        RemoteAds,
        AdInterval,
        CharacterSelection,
        CharacterGender,
        GenderSelection,
        Difficulty,
        AveragePoints,
        Tutorial,
        CollectionObject,
        GiftPercentage,
        Gift,
        EarnedGifts,
    }

    public static class UserPrefs {

        #region Player Inventory

        public static int GetGift()
        {
            return LocalPrefs.GetInt(PrefType.Gift.ToString(), 0);
        }

        public static void IncreaseGift()
        {
            var giftNo = GetGift();
            giftNo += 1;
            LocalPrefs.SetInt(PrefType.Gift.ToString(), giftNo);
            Save();
        }

        public static void AddToGifts(Gift gift)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            GiftList giftList = new GiftList();
            giftList.Gifts = new List<Gift>();
            var gifts = LocalPrefs.GetString(PrefType.EarnedGifts.ToString(), "");
            if (!string.IsNullOrEmpty(gifts))
            {
                giftList = JsonConvert.DeserializeObject<GiftList>(gifts, settings);
            }
            
            giftList.Gifts.Add(gift);

            var serializedObject = JsonConvert.SerializeObject(giftList, settings);
            LocalPrefs.SetString(PrefType.EarnedGifts.ToString(), serializedObject);
            
            Save();
        }

        public static List<Gift> GetEarnedGifts()
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var gifts = LocalPrefs.GetString(PrefType.EarnedGifts.ToString(), "");
            if (string.IsNullOrEmpty(gifts))
                return new List<Gift>();
            var giftList = JsonConvert.DeserializeObject<GiftList>(gifts, settings);
            return giftList.Gifts;
        }
        
        public static bool IsCharacterOwnable (string charName) {

            return !GetCharactersFromPlayerInventory ().Contains (charName) && !GetUnlockedCharacters().Contains(charName);
        }

        public static List<string> GetCharactersFromPlayerInventory () {
            var allChars = LocalPrefs.GetString (PrefType.PlayerOwnedCharacters.ToString (), "Default;");

            var splitted = allChars.Split (';');

            var charList = new List<string> ();

            for (int i = 0; i < splitted.Length; i++) {
                if (!string.IsNullOrEmpty (splitted[i])) {
                    charList.Add (splitted[i]);
                }

            }

            return charList;
        }

        public static List<string> GetUnlockedCharacters()
        {
            var allChars = LocalPrefs.GetString(PrefType.UnlockedCharacters.ToString(), "");

            var splitted = allChars.Split(';');

            var charList = new List<string>();

            for (int i = 0; i < splitted.Length; i++)
            {
                if (!string.IsNullOrEmpty(splitted[i]))
                {
                    charList.Add(splitted[i]);
                }

            }

            return charList;
        }

        public static void AddToUnlockedCharacters(string charName)
        {
            var allChars = LocalPrefs.GetString(PrefType.UnlockedCharacters.ToString(), "");
            if(!allChars.Contains(charName + ";"))
            { 
                allChars += charName + ";";
                LocalPrefs.SetString(PrefType.UnlockedCharacters.ToString(), allChars);
            }
            Save();
        }

        public static void AddCharacterPlayerInventory (string charName) {
            
            var allChars = LocalPrefs.GetString (PrefType.PlayerOwnedCharacters.ToString (), "Default;");
            allChars += charName + ";";
            LocalPrefs.SetString (PrefType.PlayerOwnedCharacters.ToString (), allChars);

            AddToUnlockedCharacters(charName);
        }

        public static string GetPlayerCharacter () {
            return LocalPrefs.GetString (PrefType.PlayerCharacter.ToString (), "Default");
        }

        public static void SetPlayerCharacter (string charName) {
            LocalPrefs.SetString (PrefType.PlayerCharacter.ToString (), charName);
            Save();
        }

        /// <summary>
        /// Her zaman ilk sıradaki haricindekileri döner (Default karakter hariç)
        /// </summary>
        /// <param name="allChars"></param>
        /// <returns></returns>
        public static string GetUnlockableCharacter(List<string> allChars)
        {
            for (int i = 1; i < allChars.Count; i++)
            {
                if (IsCharacterOwnable(allChars[i]))
                {
                    return allChars[i];
                }
            }
            return null;
        }


        #endregion

        #region Game Data

        public static int GetTotalCollection()
        {
            return LocalPrefs.GetInt(PrefType.CollectionObject.ToString(), 0);
        }
        public static void IncreaseCollection(int amount)
        {
            var totalAmount = GetTotalCollection() + amount;
            SetTotalCollection(totalAmount);
        }

        public static void SetTotalCollection(int totalAmount)
        {
            LocalPrefs.SetInt(PrefType.CollectionObject.ToString(), totalAmount);
            Save();
            EventManager.Instance.Invoke_CollectionUpdated();

        }

        public static float GetGiftSliderPercentage()
        {
            return LocalPrefs.GetFloat(PrefType.GiftPercentage.ToString(), 0f);
        }

        public static void ResetGiftSliderPercentage()
        {
            LocalPrefs.SetFloat(PrefType.GiftPercentage.ToString(), 0f);
        }

        public static float UpdateGiftSliderPercentage(float updateWithPercent = 5)
        {
            var sliderPercentage = GetGiftSliderPercentage();

            sliderPercentage += updateWithPercent / 100f;

            LocalPrefs.SetFloat(PrefType.GiftPercentage.ToString(), sliderPercentage >= 0.99f ? 0f : sliderPercentage);
            
            Save();
            
            return Mathf.Clamp01(sliderPercentage);
        }
        
        public static int IncreaseLevelNo (bool isBonusAvailable = true) {
            var currentLevel = GetUserLevel();
            var currentLevelNo = currentLevel.LevelNo;
            if(isBonusAvailable)
            { 
                if(currentLevelNo % 3 == 0 && !currentLevel.IsBonusLevel)
                {
                    currentLevel.IsBonusLevel = true;
                }
                else if(currentLevelNo % 3 == 0 && currentLevel.IsBonusLevel)
                {
                    currentLevelNo += 1;
                    currentLevel.LevelNo = currentLevelNo;
                    currentLevel.IsBonusLevel = false;
                }
                else
                {
                    currentLevelNo += 1;
                    currentLevel.LevelNo = currentLevelNo;
                }
            }
            else
            {
                currentLevelNo += 1;
                currentLevel.LevelNo = currentLevelNo;
            }

            SetUserLevel (currentLevel);

            return currentLevelNo;

        }
        public static void SetLevelNo(int levelNo, bool isBonus = false)
        {
            var currentLevel = GetUserLevel();
            currentLevel.LevelNo = levelNo;
            currentLevel.IsBonusLevel = isBonus;
            SetUserLevel(currentLevel);
        }
        public static int GetLevelNo()
        {
            var userLevel = GetUserLevel();
            return userLevel.LevelNo;
        }
        
        public static UserLevel GetUserLevel()
        {
            var levelString = LocalPrefs.GetString(PrefType.LevelNo.ToString(), "{\"LevelNo\": 1,\"IsBonusLevel\": false}");
            var userLevel = JsonUtility.FromJson<UserLevel>(levelString);
            return userLevel;
        }

        public static void SetUserLevel(UserLevel userLevel)
        {
            var json = JsonUtility.ToJson(userLevel);
            Log.Info("User Level : " + json);
            LocalPrefs.SetString(PrefType.LevelNo.ToString(), json);
            Save();
        }
        

        public static void SetCharacterSelection(bool isCharacterSelected)
        {
            LocalPrefs.SetBool(PrefType.CharacterSelection.ToString(), isCharacterSelected);
        }
        public static bool GetCharacterSelection()
        {
            return LocalPrefs.GetBool(PrefType.CharacterSelection.ToString(), false);
        }



        #endregion

        #region General Settings

        public static void SetTutorial(bool isTutorialOn)
        {
            LocalPrefs.SetBool(PrefType.Tutorial.ToString(), isTutorialOn);
        }

        public static bool GetTutorial()
        {
            return LocalPrefs.GetBool(PrefType.Tutorial.ToString(), false);
        }

        public static void SetSound(bool isSoundOn)
        {
            LocalPrefs.SetBool(PrefType.Sound.ToString(), isSoundOn);
        }

        internal static bool GetAdTesting()
        {
            return false;
        }

        public static bool GetSound()
        {
            return LocalPrefs.GetBool(PrefType.Sound.ToString(), true);
        }

        public static void SetVibration(bool isVibrationOn)
        {
            LocalPrefs.SetBool(PrefType.Vibration.ToString(), isVibrationOn);
        }
        public static bool GetVibration()
        {
            return LocalPrefs.GetBool(PrefType.Vibration.ToString(), true);
        }

        public static void SetTestNotif (bool isOn) {
            LocalPrefs.SetBool ("Test_Notif", isOn);
        }
        public static bool GetTestNotif () {
            return LocalPrefs.GetBool ("Test_Notif", false);
        }

        public static void SetNotification (bool isSet) {
            LocalPrefs.SetBool ("Notification_1", isSet);
        }

        public static bool GetNotification () {
            return LocalPrefs.GetBool ("Notification_1", false);
        }

        public static void SetNoAds (bool isBought) {
            LocalPrefs.SetBool (PrefType.NoAds_AdsRemoved.ToString (), isBought);
        }

        public static bool GetNoAds () {
            return LocalPrefs.GetBool (PrefType.NoAds_AdsRemoved.ToString (), false);
        }


        #endregion

        #region Remote Config

        public static void SetRemoteAdsOn(bool isOn)
        {
            LocalPrefs.SetBool(PrefType.RemoteAds.ToString(), isOn);
        }

        public static bool GetRemoteAdsOn()
        {
            return LocalPrefs.GetBool(PrefType.RemoteAds.ToString(), false);
        }

        public static void SetAdInterval(float interval)
        {
            LocalPrefs.SetFloat(PrefType.AdInterval.ToString(), interval);
        }

        public static float GetAdInterval()
        {
            return LocalPrefs.GetFloat(PrefType.AdInterval.ToString(), 0.25f);
        }

        #endregion

        public static void Save()
        {
            LocalPrefs.Save(LocalPrefs.defaultFileName, true);
        }

    }
}
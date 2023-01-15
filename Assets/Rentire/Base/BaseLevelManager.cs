using MEC;
using Rentire.Core;
using Rentire.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseLevelManager : RMonoBehaviour
{
    public Level CurrentLevel;

    [Header("Dosya / Klasör ayarları")] public string LevelsFolder = "Levels";
    public string LevelPrefix = "Level";

    [Header("Level Genel Ayarlar")] [Space(2)]
    public bool hasTutorial = false;

    public bool IsAdmin;
    public int AdminLevelNo;
    public int TotalLevelCount;
    public int CurrentLevelNo;
    public GameObject CurrentLevelGO;

    protected virtual void Awake()
    {
        CreateLevel();
    }

    public void CreateLevel()
    {
        if (TotalLevelCount <= 0)
        {
            Log.Warning("TOTAL LEVEL COUNT MUST BE SET");
            TotalLevelCount = 1;
        }

        CurrentLevelNo = UserPrefs.GetLevelNo();
#if UNITY_EDITOR
        if (IsAdmin)
            CurrentLevelNo = AdminLevelNo;
#endif

        var levelNoToCreate = CalculateLevelNo();

        var levelResource = Resources.Load<GameObject>(LevelsFolder + "/" + LevelPrefix + levelNoToCreate);
        if (levelResource != null)
        {
            CurrentLevelGO = Instantiate(levelResource, Vector3.zero, Quaternion.identity);

            if (CurrentLevel == null)
                CurrentLevel = CurrentLevelGO.GetComponent<Level>();
        }

        // If Progress updater is assigned
        if (ProgressUpdater.Instance)
            ProgressUpdater.Instance.UpdateLevelText(CurrentLevelNo);
    }

    int CalculateLevelNo()
    {
        int levelNoToCreate = CurrentLevelNo;

        if (!hasTutorial)
        {
            Log.Info("Dont have Tutorial");
            if (CurrentLevelNo > TotalLevelCount)
            {
                levelNoToCreate = CurrentLevelNo % TotalLevelCount;

                if (levelNoToCreate == 0)
                    levelNoToCreate = TotalLevelCount;
            }
        }
        else
        {
            Log.Info("Has Tutorial");
            if (CurrentLevelNo > TotalLevelCount)
            {
                var moddedLevel = CurrentLevelNo % (TotalLevelCount - 1);
                Log.Info("Modded Level Count is " + moddedLevel);
                if (moddedLevel is 0 or 1)
                {
                    levelNoToCreate = moddedLevel + (TotalLevelCount - 1);
                    Log.Info("Created Level " + levelNoToCreate);
                }
                else
                {
                    levelNoToCreate = moddedLevel;
                }

                // Safety Check
                if (levelNoToCreate > TotalLevelCount)
                    levelNoToCreate = TotalLevelCount;
            }
        }


        return levelNoToCreate;
    }

    public void IncreaseLevelNo(bool isBonusAvailable = false)
    {
        UserPrefs.IncreaseLevelNo(isBonusAvailable);
    }

    public void RestartLevel()
    {
        Timing.KillCoroutines();
        StopAllCoroutines();

        string activeScene = SceneManager.GetActiveScene().name;

        if (RSceneLoader.Instance)
            RSceneLoader.Instance.ChangeScene(activeScene);
        else
            SceneManager.LoadScene(activeScene);
    }

    public void NextLevel()
    {
        Log.Info("Loading next level");
        RestartLevel();
    }
}
using System.Collections;
using System.Collections.Generic;
#if dUI_MANAGER
using Doozy.Engine.UI;
#endif
using MEC;
using Rentire.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RSceneLoader : Singleton<RSceneLoader>
{

    public int CurrentIndex { get { return currentSceneIndex; } }
#if dUI_MANAGER
    public UIView LoadingView;
#endif
    int currentSceneIndex = 0;
    AsyncOperation asyncSceneLoad;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.activeSceneChanged += ActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentSceneIndex = 0;    
    }
    
    public void ChangeScene(string sceneName)
    {
        ChangeScene(sceneName, null);
    }

    public void ChangeScene(int sceneIndex)
    {
        ChangeScene(null, sceneIndex);
    }

    public void ChangeSceneAsync(string sceneName)
    {
        ChangeSceneAsync(sceneName, null);
    }

    public void ChangeSceneAsync(int sceneIndex)
    {
        ChangeSceneAsync(null, sceneIndex);
    }

    void ChangeScene(string sceneName, int? sceneIndex)
    {
        int buildSceneIndex = 0;
        if(!string.IsNullOrEmpty(sceneName))
        {
            buildSceneIndex = SceneManager.GetSceneByName(sceneName.Trim()).buildIndex;
        }
        else if(sceneIndex.HasValue)
        {
            buildSceneIndex = sceneIndex.Value;
        }
#if dUI_MANAGER
        LoadingView.Show();
#endif

        StartCoroutine(LoadScene(buildSceneIndex, 1.5f));
    }

    void ChangeSceneAsync(string sceneName, int? sceneIndex)
    {
        int buildSceneIndex = 0;
        if (!string.IsNullOrEmpty(sceneName))
        {
            buildSceneIndex = SceneManager.GetSceneByName(sceneName.Trim()).buildIndex;
        }
        else if (sceneIndex.HasValue)
        {
            buildSceneIndex = sceneIndex.Value;
        }
#if dUI_MANAGER
        LoadingView.Show();
#endif

        StartCoroutine(LoadSceneAsync(buildSceneIndex));
    }

    IEnumerator LoadSceneAsync(int buildIndex)
    {
        asyncSceneLoad = SceneManager.LoadSceneAsync(buildIndex);
        // Wait until the asynchronous scene fully loads
        while (!asyncSceneLoad.isDone)
        {
            yield return null;
        }
    }
    IEnumerator LoadScene(int buildIndex, float secondsToWait)
    {
        yield return new WaitForSecondsRealtime(secondsToWait);
        SceneManager.LoadScene(buildIndex);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        Log.Info("Scene Loaded : " + scene.name);
        currentSceneIndex = scene.buildIndex;
#if dUI_MANAGER
        LoadingView.Hide();
#endif
    }

    private void ActiveSceneChanged(Scene prevScene, Scene newScene)
    {
        Log.Info(string.Format( "Active Scene {0} Changed To {1}",prevScene.name,newScene.name));
#if dUI_MANAGER
        LoadingView.Hide();
#endif
    }
}

using System.Collections;
using System.Collections.Generic;
//using Doozy.Engine.UI;
using Rentire.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : Singleton<SceneChanger>
{
    //public UIView View_Change;

    private AsyncOperation asyncOperation;

    public void StartChangingScene(string sceneName)
    {
        //View_Change.Show();
        CallMethodWithDelay(() => {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = true;
        }, .5f);
        
    }


    public void ChangeSceneEnded()
    {
        //View_Change.Hide();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public Image loadingImage;
    public bool baslangic;
    public GameObject levelLoadingScene;
    void Start()
    {
        if (baslangic)
        {
            PlayerPrefs.SetInt("ekranGenislik", Screen.currentResolution.width);
            PlayerPrefs.SetInt("ekranYukseklik", Screen.currentResolution.height);
#if UNITY_ANDROID
            Screen.SetResolution(Screen.currentResolution.width / 3, Screen.currentResolution.height / 3, true);
#endif

        }
        Baslat();
    }

    void Baslat()
    {
        LoadLevel(1);

    }
    public void LoadLevelString(string sceneIndex)
    {
        StartCoroutine(LoadAsynchronouslyString(sceneIndex));
        if (levelLoadingScene != null)
        {
            levelLoadingScene.SetActive(true);
        }
    }
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
        if (levelLoadingScene != null)
        {
            levelLoadingScene.SetActive(true);
        }
    }
    private void Update()
    {
        // ortaBar.transform.Rotate(0, 0, -150*Time.deltaTime);

    }
    IEnumerator LoadAsynchronouslyString(string sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            if (loadingImage != null)
            {
                float yuzde = Mathf.Clamp01(operation.progress / .9f);
             //   loadingImage.fillAmount = yuzde;
            }
            //ortaBar.transform.Rotate(0, 0,5);
            yield return null;
        }
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            if (loadingImage != null)
            {
                float yuzde = Mathf.Clamp01(operation.progress / .9f);
             //   loadingImage.fillAmount = yuzde;
            }
            //ortaBar.transform.Rotate(0, 0,5);
            yield return null;
        }
    }
}

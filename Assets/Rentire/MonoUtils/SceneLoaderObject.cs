using System.Collections;
using Rentire.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderObject : MonoBehaviour {
    public string SceneName;

    public float SecondsToWait = 3f;

    private AsyncOperation loadSceneOperation;
    // Start is called before the first frame update
    void Start () {
        
        StartCoroutine (_StartLoadingScene ());
    }

    public IEnumerator _StartLoadingScene () {
        yield return new WaitForSeconds (SecondsToWait);
        SceneManager.LoadSceneAsync(SceneName).allowSceneActivation = true;
        //SceneChanger.Instance.StartChangingScene(SceneName);
        /*
        SceneChanger.Instance.SceneChangeStarted (null, () => {
            SceneManager.LoadSceneAsync (SceneName);
        });
        */
    }

}
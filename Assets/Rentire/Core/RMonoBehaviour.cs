using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MEC;
using Rentire.Core;
using UnityEngine;
using UnityEngine.Diagnostics;

public class RMonoBehaviour : MonoBehaviour {

    protected GameManager gameManager => GameManager.Instance;
    protected GameState gameState => gameManager.CurrentGameState;
    protected Player player => Player.Instance;
    protected PlayerState currentPlayerState => player.PlayerStateManager.currentPlayerState;

    protected EventManager eventManager => EventManager.Instance;

    #region Static Methods

    public static T LoadAndInstantiate<T>(string path, GameObject root) where T: Component
    {
        var resObject = Resources.Load<T>(path);

        return Instantiate(resObject, root);
    }

    public static T LoadAndInstantiate<T>(string path, GameObject root, Vector3 localPosition, Quaternion localRotation) where T : Component
    {
        var resObject = Resources.Load<T>(path);

        return Instantiate(resObject, root, localPosition, localRotation);
    }

    public static T Load<T>(string path) where T : Component
    {
        return Resources.Load<T>(path);
    }
    
    public static T LoadObject<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    /// <summary>
    /// Instantiates a prefab and attaches it to the given root. 
    /// </summary>
    public static T Instantiate<T> (T prefab, GameObject root) where T : Component {
        var newObject = Instantiate (prefab);

        newObject.transform.SetParent (root.transform, false);
        newObject.transform.ResetLocal ();

        return newObject;
    }

    /// <summary>
    /// Instantiates a prefab, attaches it to the given root, and
    /// sets the local position and rotation.
    /// </summary>
    public static T Instantiate<T> (T prefab, GameObject root, Vector3 localPosition, Quaternion localRotation) where T : Component {
        var newObject = Instantiate<T> (prefab);

        newObject.transform.parent = root.transform;

        newObject.transform.localPosition = localPosition;
        newObject.transform.localRotation = localRotation;
        newObject.transform.ResetScale ();

        return newObject;
    }

    public static GameObject LoadAndInstantiate(string path, GameObject root)
    {
        var resObject = Resources.Load<GameObject>(path);

        return Instantiate(resObject, root);
    }

    public static GameObject LoadAndInstantiate(string path, GameObject root, Vector3 localPosition, Quaternion localRotation)
    {
        var resObject = Resources.Load<GameObject>(path);

        return Instantiate(resObject, root, localPosition, localRotation);
    }

    /// <summary>
    /// Instantiates a prefab.
    /// </summary>
    /// <param name="prefab">The object.</param>
    /// <returns>GameObject.</returns>
    public static GameObject Instantiate (GameObject prefab) {
        return Object.Instantiate (prefab);
    }

    /// <summary>
    /// Instantiates the specified prefab.
    /// </summary>
    public static GameObject Instantiate (GameObject prefab, Vector3 position, Quaternion rotation) {
        var newObject = Object.Instantiate (prefab, position, rotation);

        return newObject;
    }

    /// <summary>
    /// Instantiates a prefab and parents it to the root.
    /// </summary>
    /// <param name="prefab">The prefab.</param>
    /// <param name="root">The root.</param>
    /// <param name="isCanvas">To know if it is instantiate in a canvas or not</param>
    /// <returns>GameObject.</returns>
    public static GameObject Instantiate (GameObject prefab, GameObject root) {
        var newObject = (GameObject) Object.Instantiate (prefab);

        newObject.transform.parent = root.transform;

        newObject.transform.ResetLocal ();

        return newObject;
    }

    /// <summary>
    /// Instantiates a prefab, attaches it to the given root, and
    /// sets the local position and rotation.
    /// </summary>
    /// <param name="prefab">The prefab.</param>
    /// <param name="root">The root.</param>
    /// <param name="localPosition">The local position.</param>
    /// <param name="localRotation">The local rotation.</param>
    /// <returns>GameObject.</returns>
    public static GameObject Instantiate (GameObject prefab, GameObject root, Vector3 localPosition, Quaternion localRotation) {
        var newObject = (GameObject) Object.Instantiate (prefab, root.transform, true);

        newObject.transform.localPosition = localPosition;
        newObject.transform.localRotation = localRotation;
        newObject.transform.ResetScale ();

        return newObject;
    }

    #region Find

    /// <summary>
    /// Similar to FindObjectsOfType, except that it looks for components
    /// that implement a specific interface.
    /// </summary>
    public static List<I> FindObjectsOfInterface<I> () where I : class {
        var monoBehaviours = FindObjectsOfType<MonoBehaviour> ();

        return monoBehaviours.Select (behaviour => behaviour.GetComponent (typeof (I))).OfType<I> ().ToList ();
    }

    #endregion

    public static float Fmap(float value, float fromLow, float fromHigh, float toLow, float toHigh)
    {
        return Mathf.Clamp((value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow, toLow, toHigh);
    }

    #endregion

    
    /// <summary>
    /// This method is used for checking whether the component or object is assigned on inspector or not
    /// </summary>
    /// <param name="obj"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool CheckIfObjectAssigned<T> (T obj) where T : Component {
        if (obj == null) {
            var callerMethodName = new StackFrame (1).GetMethod ().ReflectedType.Name;
            Log.Error (string.Format ("{0} : The component {1} is not assigned!", callerMethodName, obj.GetType ().Name));
            return false;
        }
        return true;

    }

    public void GetComponentAndAssign<T>(ref T obj, bool includeChildren = false, bool includeInactive = false) where T : Component
    {
        if (obj == null)
        {
            obj = includeChildren ? GetComponentInChildren<T>(includeInactive) : GetComponent<T>();
        }
    }

    public void CallMethodWithDelay (System.Action action, float delay) {
        if (action != null) {
            Timing.CallDelayed (delay, action);
        }
    }
    
    protected void CallMethod(System.Action action, float delay)
    {
        StartCoroutine(_CallMethod(action, delay));
    }

    protected Coroutine CallMethodRealtime(System.Action action, float delay)
    {
        return StartCoroutine(_CallMethod(action, delay,true));
    }

    protected Coroutine CallMethodContinuously(System.Action action, float timePeriod)
    {
        return StartCoroutine(_CallMethodContinuously(action, timePeriod));
    }
    protected Coroutine CallMethodPeriodically(System.Action action, float totalTime, float timePeriod, System.Action onComplete = null)
    {
        return StartCoroutine(_CallMethodPeriodically(action, totalTime, timePeriod, onComplete));
    }

    IEnumerator _CallMethodContinuously(System.Action action, float timePeriod)
    {
        var waitFor = new WaitForSeconds(timePeriod);
        while(true)
        {
            yield return waitFor;
            if(action != null)
            {
                action.Invoke();
            }
        }
    }

    IEnumerator _CallMethodPeriodically(System.Action action, float totalTime, float timePeriod, System.Action onComplete = null)
    {
        var totalLoop = totalTime / timePeriod;
        var waitFor = new WaitForSeconds(timePeriod);
        for (int i = 0; i < totalLoop; i++)
        {
            yield return waitFor;
            if(action != null)
            {
                action.Invoke();
            }    
        }
        if(onComplete != null)
            onComplete.Invoke();
        
    }

    IEnumerator _CallMethod(System.Action action, float delay, bool isRealtime = false)
    {
        if(!isRealtime)
            yield return new WaitForSeconds(delay);
        else
            yield return new WaitForSecondsRealtime(delay);

        if (action != null)
        {
            action.Invoke();
        }
    }

    protected Coroutine CountdownRealtime(int seconds, int tickPeriod, System.Action<int> onTick, System.Action onFinished)
    {
        return StartCoroutine(_Countdown(seconds, tickPeriod, onTick, onFinished));
    }

    protected Coroutine CountdownRealtime(float seconds, float tickPeriod, System.Action<float> onTick, System.Action onFinished)
    {
        return StartCoroutine(_Countdown(seconds, tickPeriod, onTick, onFinished));
    }

    IEnumerator _Countdown(int seconds, int tickPeriod, System.Action<int> onTick, System.Action onFinished)
    {
        while(seconds > 0)
        {
            yield return new WaitForSecondsRealtime(tickPeriod);
            seconds -= tickPeriod;
            if (onTick != null)
                onTick.Invoke(seconds);
        }
        if(onFinished != null)
        {
            onFinished.Invoke();
        }
    }

    IEnumerator _Countdown(float seconds, float tickPeriod, System.Action<float> onTick, System.Action onFinished)
    {
        while(seconds > 0)
        {
            yield return new WaitForSecondsRealtime(tickPeriod);
            seconds -= tickPeriod;
            if (onTick != null)
                onTick.Invoke(seconds);
        }
        if(onFinished != null)
        {
            onFinished.Invoke();
        }
    }

    public void LerpTo(object LerpObject, float endValue,float second)
    {
#if DOTWEEN_API
        DG.Tweening.DOTween.To(() => (float)LerpObject, x => LerpObject = x, endValue, second);
#endif
    }
}
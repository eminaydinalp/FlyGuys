using Rentire.Core;
using UnityEngine;

public class NoThanksUI : RMonoBehaviour
{
    public GameObject NoThanksButton;
    bool disable = false;

    private void Awake()
    {
        if (!NoThanksButton)
            Log.Error("No Thanks button is not assigned!");
        NoThanksButton.SetActive(false);
    }

    private void OnEnable()
    {
        var waitingPeriod = RemoteManager.Instance.GetNoThanksFrequency();
        CallMethodRealtime(() => NoThanksButton.SetActive(!disable), waitingPeriod);
    }

    public void Disable()
    {
        disable = true;
    }

    private void OnDisable()
    {
        NoThanksButton.SetActive(false);
    }
}

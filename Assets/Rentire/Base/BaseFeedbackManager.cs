#if MOREMOUNTAINS_NICEVIBRATIONS
using MoreMountains.NiceVibrations;
using Rentire.Core;
#endif

public abstract class BaseFeedbackManager : RMonoBehaviour
{
#if MOREMOUNTAINS_NICEVIBRATIONS
    public void Vibrate(HapticTypes hapticType)
    {
        MoreMountains.NiceVibrations.MMVibrationManager.Haptic(hapticType);
        Log.Info("Vibrated : " + hapticType);
    }
#endif
}

using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Rentire.Core;
using Sirenix.OdinInspector;

public class ShakeManager : Singleton<ShakeManager>,IGameStateObserver
{

    public List<CameraShakeObject> shakeObjects = new List<CameraShakeObject>();
    
    #region Shake

    public void CameraShake(ShakeType _shakeType)
    {
        var selectedShake = shakeObjects.FirstOrDefault(x => x.shakeType == _shakeType);
        if (selectedShake != null)
        {
            selectedShake.impulseSource.GenerateImpulse();
        }
        else
        {
            Log.Error("Selected shake is null!");
        }
    }
   

    #endregion


    #region State Changed

    public void AddToGameObserverList()
    {
        gameManager.AddListener(this);
    }
    

    public void OnGameStateChanged()
    {
        if (gameState == GameState.Fail)
            CameraShake(ShakeType.ObstacleCollision);
    }

    #endregion



}
[System.Serializable]
public class CameraShakeObject
{
    public ShakeType shakeType;
    public CinemachineImpulseSource impulseSource;
}

public enum ShakeType
{
    Light,
    Medium,
    Hard,
    VeryHard,
    ObstacleCollision,
}
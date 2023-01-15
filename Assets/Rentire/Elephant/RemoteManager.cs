using System;
using ElephantSDK;
using Rentire.Core;

public class RemoteManager : Singleton<RemoteManager>
{
    public float default_no_thanks_frequency = 2f;
    public float default_movement_factor = 10f;
    public float GetNoThanksFrequency()
    {
         var frequency = ElephantSDK.RemoteConfig.GetInstance().GetFloat(Remotes.no_thanks_frequency.ToString(), default_no_thanks_frequency);
         return frequency;
    }

    public float GetPlayerMovementFactor()
    {
        var mf = ElephantSDK.RemoteConfig.GetInstance().GetFloat(Remotes.movement_factor.ToString(), default_movement_factor);
        return mf;
    }
    
    public string GetStringWithKey(Remotes key, string def)
    {
        var str = RemoteConfig.GetInstance().Get(key.ToString(), def);
        return str;
    }
}

public static class RRemote
{
    public static T Get<T>(Remotes remote, T defaultValue)
    {
        if (defaultValue is bool)
        {
            return ChangeType<T>(RemoteConfig.GetInstance()
                .GetBool(remote.ToString(),
                    bool.Parse(defaultValue
                        .ToString())));
        }

        if (defaultValue is float)
        {
            return ChangeType<T>(RemoteConfig.GetInstance()
                .GetFloat(remote.ToString(), float.Parse(defaultValue.ToString())));
        }

        if (defaultValue is string)
        {
            return ChangeType<T>(RemoteConfig.GetInstance().Get(remote.ToString(), defaultValue.ToString()));
        }

        if (defaultValue is int)
        {
            return ChangeType<T>(RemoteConfig.GetInstance()
                .GetInt(remote.ToString(), int.Parse(defaultValue.ToString())));
        }

        if (defaultValue is double)
        {
            return ChangeType<T>(RemoteConfig.GetInstance()
                .GetDouble(remote.ToString(), double.Parse(defaultValue.ToString())));
        }

        if (defaultValue is long)
        {
            return ChangeType<T>(RemoteConfig.GetInstance()
                .GetLong(remote.ToString(), long.Parse(defaultValue.ToString())));
        }

        return default;
    }

    static T ChangeType<T>(object val)
    {
        return (T) Convert.ChangeType(val, typeof(T));
    }
}

public enum Remotes
{
    no_thanks_frequency,
    movement_factor,
}

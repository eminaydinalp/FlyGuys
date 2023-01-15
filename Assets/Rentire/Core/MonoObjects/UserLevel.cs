using UnityEngine;

public class UserLevel 
{
    public UserLevel(int levelNo, bool isBonusLevel)
    {
        LevelNo = levelNo;
        IsBonusLevel = isBonusLevel;
    }

    public int LevelNo;
    public bool IsBonusLevel;

}

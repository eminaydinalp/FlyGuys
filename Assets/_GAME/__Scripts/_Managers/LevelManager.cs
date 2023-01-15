public class LevelManager : BaseLevelManager
{
    public static LevelManager Instance;

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
            Instance = this;
    }
}

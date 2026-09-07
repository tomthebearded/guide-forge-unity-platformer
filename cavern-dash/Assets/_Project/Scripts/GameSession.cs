using UnityEngine;

public class GameSession : MonoBehaviour
{
    private static GameSession instance;

    public static GameSession Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject holder = new(nameof(GameSession));
                instance = holder.AddComponent<GameSession>();
                DontDestroyOnLoad(holder);
            }

            return instance;
        }
    }

    public int TotalCoins { get; private set; }
    public float ElapsedSeconds { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewRun()
    {
        TotalCoins = 0;
        ElapsedSeconds = 0f;
    }

    public void AddCoins(int count)=>
        TotalCoins += count;

    public void AddTime(float seconds)=>
        ElapsedSeconds += seconds;
}

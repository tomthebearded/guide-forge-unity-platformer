using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public const string BestTimeKey = "cavernDash.run.bestTimeMs";

    [SerializeField] private TMP_Text timeLabel;
    [SerializeField] private TMP_Text bestTimeLabel;
    [SerializeField] private TMP_Text coinsLabel;

    private void Start()
    {
        int runMilliseconds = Mathf.RoundToInt(GameSession.Instance.ElapsedSeconds * 1000f);

        int bestMilliseconds = PlayerPrefs.GetInt(BestTimeKey, int.MaxValue);

        if (runMilliseconds < bestMilliseconds)
        {
            bestMilliseconds = runMilliseconds;
            PlayerPrefs.SetInt(BestTimeKey, 
            bestMilliseconds);

            PlayerPrefs.Save();
        }

        timeLabel.text = $"Time  {LevelTimer.FormatTime(runMilliseconds)}";
        bestTimeLabel.text = $"Best  {LevelTimer.FormatTime(bestMilliseconds)}";
        coinsLabel.text = $"Coins  {GameSession.Instance.TotalCoins}";
    }
}

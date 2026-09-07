using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeLabel;

    private void Update()
    {
        GameSession.Instance.AddTime(Time.deltaTime);

        timeLabel.text = FormatTime(Mathf.RoundToInt(GameSession.Instance.ElapsedSeconds * 1000f));
    }

    public static string FormatTime(int milliseconds)
    {
        int totalSeconds = milliseconds / 1000;
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        int hundredths = milliseconds % 1000 / 10;

        return $"{minutes:00}:{seconds:00}.{hundredths:00}";
    }
}

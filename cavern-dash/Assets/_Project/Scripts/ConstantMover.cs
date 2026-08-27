using UnityEngine;

public class ConstantMover : MonoBehaviour
{
    [Header("Framerate Test")]
    [SerializeField] private int targetFrameRateForTesting = 60;
    private float nextReportTimeSeconds = 1f;
    [SerializeField] private float moveSpeedUnitPerSeconds = 3f;


    void Awake()
    {
        Application.targetFrameRate = targetFrameRateForTesting;
    }

    void Update()
    {
        transform.position += Vector3.right * (moveSpeedUnitPerSeconds * Time.deltaTime);
        if (Time.time > nextReportTimeSeconds)
        {
            float achievedSpeedUnitsPerSecond = transform.position.x / Time.time;
            Debug.Log($"t={Time.time:F2}s  x={transform.position.x:F2}  x/t={achievedSpeedUnitsPerSecond:F1}  frame={Time.deltaTime * 1000f:F0} ms");
            nextReportTimeSeconds += 1f;
        }
    }
}

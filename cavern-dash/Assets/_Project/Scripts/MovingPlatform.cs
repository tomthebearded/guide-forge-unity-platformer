using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector2 travelOffsetUnits = new(6f, 0f);
    [SerializeField] private float speedUnitsPerSecond = 2f;

    private Rigidbody2D body;
    private Vector2 startPosition;
    private Vector2 endPosition;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        startPosition = body.position;
        endPosition = startPosition + travelOffsetUnits;
    }

    private void FixedUpdate()
    {
        float distanceAlongPath = Mathf.PingPong(Time.time * speedUnitsPerSecond, travelOffsetUnits.magnitude);

        Vector2 target = Vector2.Lerp(startPosition, endPosition, distanceAlongPath / travelOffsetUnits.magnitude);

        body.MovePosition(target);
    }
}

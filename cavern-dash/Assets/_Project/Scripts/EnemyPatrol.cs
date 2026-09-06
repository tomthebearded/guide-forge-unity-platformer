using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private float speedUnitsPerSecond = 2f;
    [SerializeField] private LayerMask groundLayers;

    [Header("Probes")]
    [SerializeField] private float probeDistanceAheadUnits = 0.45f;
    [SerializeField] private float floorProbeDepthUnits = 0.55f;
    [SerializeField] private float probeRadiusUnits = 0.08f;

    private Rigidbody2D body;

    private int direction = 1;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!IsFloorAhead() || IsWallAhead())
            direction = -direction;

        Vector2 step = Vector2.right * (direction * speedUnitsPerSecond * Time.fixedDeltaTime);
        body.MovePosition(body.position + step);
    }

    private Vector2 AheadPosition() =>
         body.position + Vector2.right * (direction * probeDistanceAheadUnits);

    private bool IsFloorAhead()
    {
        Vector2 probe = AheadPosition() + Vector2.down * floorProbeDepthUnits;
        return Physics2D.OverlapCircle(probe, probeRadiusUnits, groundLayers) != null;
    }

    private bool IsWallAhead() =>
         Physics2D.OverlapCircle(AheadPosition(), probeRadiusUnits, groundLayers) != null;
}

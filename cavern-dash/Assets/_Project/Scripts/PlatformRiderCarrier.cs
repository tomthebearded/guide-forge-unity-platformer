using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlatformRiderCarrier : MonoBehaviour
{
    [SerializeField] private LayerMask riderLayers;
    [SerializeField] private float standingToleranceUnits = 0.1f;

    private Collider2D platformCollider;
    private Rigidbody2D platformBody;

    private Vector2 previousPlatformPosition;

    private readonly Collider2D[] overlapResults = new Collider2D[8];
    private ContactFilter2D riderFilter;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        platformBody = GetComponent<Rigidbody2D>();
        previousPlatformPosition = platformBody.position;

        if (platformCollider.sharedMaterial == null)
        {
            platformCollider.sharedMaterial = new PhysicsMaterial2D("RiderCarrierFrictionless")
            {
                friction = 0f,
                bounciness = 0f
            };
        }

        riderFilter = new ContactFilter2D
        {
            useLayerMask = true,
            layerMask = riderLayers,
            useTriggers = false
        };
    }

    private void FixedUpdate()
    {
        Vector2 platformDelta = platformBody.position - previousPlatformPosition;
        previousPlatformPosition = platformBody.position;

        if (platformDelta == Vector2.zero)
            return;

        Bounds platformBounds = platformCollider.bounds;
        Vector2 stripCentre = new(platformBounds.center.x, platformBounds.max.y + standingToleranceUnits * 0.5f);
        Vector2 stripSize = new(platformBounds.size.x, standingToleranceUnits);

        int found = Physics2D.OverlapBox(stripCentre, stripSize, 0f, riderFilter, overlapResults);
        for (int i = 0; i < found; i++)
        {
            Rigidbody2D rider = overlapResults[i].attachedRigidbody;
            if (rider == null)
                continue;

            rider.position += platformDelta;
        }
    }
}

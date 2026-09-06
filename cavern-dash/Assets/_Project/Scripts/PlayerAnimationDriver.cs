using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Rigidbody2D))]

// The one bridge from movement to animation: it publishes parameters and nothing
// else. It never names a clip, so the controller stays free to change.
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimationDriver : MonoBehaviour
{
    private Animator animator;
    private PlayerMotor motor;
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;

    // Hashing the names once is the idiomatic way to set parameters: comparing
    // ints every frame beats comparing strings every frame.
    private static readonly int SpeedId = Animator.StringToHash("Speed");
    private static readonly int IsGroundedId = Animator.StringToHash("IsGrounded");
    private static readonly int VerticalVelocityId = Animator.StringToHash("VerticalVelocity");
    private static readonly int IsDashingId = Animator.StringToHash("IsDashing");

    private void Awake()
    {
        animator = GetComponent<Animator>();
        motor = GetComponent<PlayerMotor>();
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        animator.SetFloat(SpeedId, Mathf.Abs(body.linearVelocity.x));
        animator.SetBool(IsGroundedId, motor.IsGrounded);
        animator.SetFloat(VerticalVelocityId, body.linearVelocity.y);
        animator.SetBool(IsDashingId, motor.IsDashing);

        if (Mathf.Abs(body.linearVelocity.x) > 0.1f)
            spriteRenderer.flipX = body.linearVelocity.x > 0f;
    }
}

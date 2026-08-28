using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public float HorizontalInput { get; private set; }

    private float lastJumpPressedTimeSeconds = float.NegativeInfinity;
    public float TimeSinceJumpPressedSeconds => Mathf.Max(0f, Time.time - lastJumpPressedTimeSeconds);
    public bool IsJumpHeld { get; private set; }
    private InputAction moveAction;
    private InputAction jumpAction;



    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
    }

    void Update()
    {
        HorizontalInput = moveAction.ReadValue<Vector2>().x;

        if (jumpAction.WasPressedThisFrame())
        {
            lastJumpPressedTimeSeconds = Time.time;
        }
        
        IsJumpHeld = jumpAction.IsPressed();
    }

    public void ConsumeJumpRequest() =>
        lastJumpPressedTimeSeconds = float.NegativeInfinity;
}

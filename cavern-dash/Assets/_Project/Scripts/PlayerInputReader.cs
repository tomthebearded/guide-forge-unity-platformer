using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public bool DashRequested { get; private set; }
    public float TimeSinceJumpPressedSeconds => Mathf.Max(0f, Time.time - lastJumpPressedTimeSeconds);
    public bool IsJumpHeld { get; private set; }

    private float lastJumpPressedTimeSeconds = float.NegativeInfinity;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;



    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
        jumpAction = InputSystem.actions.FindAction("Player/Jump");
        dashAction = InputSystem.actions.FindAction("Player/Dash");
    }

    void Update()
    {
        HorizontalInput = moveAction.ReadValue<Vector2>().x;

        if (jumpAction.WasPressedThisFrame())
            lastJumpPressedTimeSeconds = Time.time;

        IsJumpHeld = jumpAction.IsPressed();

        if (dashAction.WasPressedThisFrame())
            DashRequested = true;
    }

    public void ConsumeJumpRequest() => lastJumpPressedTimeSeconds = float.NegativeInfinity;

    public void ConsumeDashRequest() => DashRequested = false;

}

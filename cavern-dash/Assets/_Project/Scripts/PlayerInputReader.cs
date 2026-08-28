using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    public bool JumpRequested { get; private set; }

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
            JumpRequested = true;
    }

    public void ConsumeJumpRequest()
    {
        JumpRequested = false;
    }
}

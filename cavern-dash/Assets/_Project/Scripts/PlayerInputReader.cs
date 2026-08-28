using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    private InputAction moveAction;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
    }

    void Update()
    {
        HorizontalInput = moveAction.ReadValue<Vector2>().x;
    }
}

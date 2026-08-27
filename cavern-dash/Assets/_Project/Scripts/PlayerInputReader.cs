using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float HorizontalInput { get; private set; }
    private InputAction moveAction;

    private float lastLoggedHorizontalInput = float.NaN;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Player/Move");
    }

    void Update()
    {
        HorizontalInput = moveAction.ReadValue<Vector2>().x;

        if (!Mathf.Approximately(HorizontalInput, lastLoggedHorizontalInput))
        {
            Debug.Log($"HorizontalInput = {HorizontalInput:F2}");
            lastLoggedHorizontalInput = HorizontalInput;
        }
    }
}

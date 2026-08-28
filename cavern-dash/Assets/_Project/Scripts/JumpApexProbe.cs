// Assets/_Project/Scripts/JumpApexProbe.cs — the whole file
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class JumpApexProbe : MonoBehaviour
{
    private PlayerMotor motor;
    private bool wasGroundedLastStep = true;
    private float heightWhenLeavingGroundUnits;
    private float highestHeightSinceLeavingUnits;


    private void Awake()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void FixedUpdate()
    {
        bool isGroundedNow = motor.IsGrounded;
        float currentHeightUnits = transform.position.y;

        if (wasGroundedLastStep && !isGroundedNow)
        {
            heightWhenLeavingGroundUnits = currentHeightUnits;
            highestHeightSinceLeavingUnits = currentHeightUnits;
        }
        else if (!isGroundedNow)
        {
            highestHeightSinceLeavingUnits = Mathf.Max(highestHeightSinceLeavingUnits, currentHeightUnits);
        }
        else if (!wasGroundedLastStep)
        {
            float apexUnits = highestHeightSinceLeavingUnits - heightWhenLeavingGroundUnits;
            Debug.Log($"apex = {apexUnits:F2} units");
        }

        wasGroundedLastStep = isGroundedNow;
    }
}
using UnityEngine;

public class ConstantMover : MonoBehaviour
{
    [SerializeField] private float moveSpeedUnitPerSeconds = 3f;

    void Update()
    {
        transform.position += Vector3.right * (moveSpeedUnitPerSeconds * Time.deltaTime);
    }
}

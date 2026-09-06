using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float parallaxFactor = 0.3f;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 cameraMovement = cameraTransform.position - previousCameraPosition;

        transform.position += new Vector3(cameraMovement.x, cameraMovement.y, 0f) * (1f - parallaxFactor);

        previousCameraPosition = cameraTransform.position;
    }
}

using UnityEngine;

public class ThirdPerson_Camera : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(-0.1f, 1.7f, -4f);
    
    [Header("Zoom Settings")]
    [SerializeField] private float minZoomDistance = 2f;
    [SerializeField] private float maxZoomDistance = 8f;
    [SerializeField] private float zoomSpeed = 1f;
    private float currentZoom;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private float minVerticalAngle = -30f;
    [SerializeField] private float maxVerticalAngle = 60f;
    private float currentRotationX = 20f;

    [Header("Smoothing")]
    [SerializeField] private float positionSmoothSpeed = 10f;
    [SerializeField] private float rotationSmoothSpeed = 10f;
    private Vector3 currentVelocity;

    // Reference to the player controller with correct script name
    private ThirdPers_PlayerController playerController;

    private void Start()
    {
        currentZoom = offset.magnitude;
        
        if (target != null)
        {
            playerController = target.GetComponent<ThirdPers_PlayerController>();
            if (playerController == null)
            {
                Debug.LogWarning("ThirdPers_PlayerController not found on target!");
            }
        }
        else
        {
            Debug.LogWarning("No target assigned to ThirdPerson_Camera!");
        }
    }

    private void LateUpdate()
    {
        if (!target || !playerController) return;

        HandleZoom();
        HandleRotation();
        UpdateCameraPosition();
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom = Mathf.Clamp(currentZoom - scrollInput * zoomSpeed, minZoomDistance, maxZoomDistance);
    }

    private void HandleRotation()
    {
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Update vertical rotation (camera only)
            currentRotationX = Mathf.Clamp(currentRotationX - mouseY * rotationSpeed, 
                                         minVerticalAngle, maxVerticalAngle);

            // Update horizontal rotation and sync with player
            playerController.AddRotation(mouseX * rotationSpeed);
        }
    }

    private void UpdateCameraPosition()
    {
        // Use player's Y rotation and camera's X rotation
        Quaternion targetRotation = Quaternion.Euler(currentRotationX, target.eulerAngles.y, 0);

        // Calculate desired position
        Vector3 normalizedOffset = offset.normalized * currentZoom;
        Vector3 targetPosition = target.position + targetRotation * normalizedOffset;

        // Smooth position transition
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref currentVelocity,
            1f / positionSmoothSpeed
        );

        // Smooth rotation transition
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSmoothSpeed
        );
    }

    public void ResetCamera()
    {
        currentRotationX = 20f;
    }
}
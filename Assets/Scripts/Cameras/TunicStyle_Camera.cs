using UnityEngine;

public class TunicStyle_Camera : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform target;
    
    [Header("Camera Configuration")]
    [SerializeField] private float distance = 12f;        // Distance from target
    [SerializeField] private float height = 10f;          // Height above target
    [SerializeField] private float smoothSpeed = 0.125f;  // Lower = smoother, Higher = more responsive
    
    [Header("Tunic-Style View Settings")]
    [SerializeField] private float pitchAngle = 50f;      // Overhead angle
    [SerializeField] private float yawAngle = -45f;       // Side angle
    
    private Vector3 currentVelocity;
    
    private void Start()
    {
        if (target == null)
        {
            Debug.LogWarning("No target assigned to Isometric_Camera. Please assign a target in the inspector.");
            target = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        // Calculate rotation
        Quaternion rotation = Quaternion.Euler(pitchAngle, yawAngle, 0);
        
        // Calculate offset from target
        Vector3 offset = new Vector3(0, height, -distance);
        Vector3 rotatedOffset = rotation * offset;
        
        // Calculate desired position
        Vector3 desiredPosition = target.position + rotatedOffset;
        
        // Smoothly move camera to desired position
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            desiredPosition, 
            ref currentVelocity, 
            smoothSpeed
        );
        
        // Maintain fixed rotation
        transform.rotation = rotation;
    }
}
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    [Header("Door")]
    [SerializeField] private Transform doorTransform;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float maxRotationAngle = 90f;
    [SerializeField] private bool isKey;

    [Header("UI")]
    [SerializeField] private GameObject interact;


    private bool isOpen = false;
    private bool inRange = false;
    private float currentRotation = 0f;
    private float targetRotation;
    private Vector3 initialRotation;

// ----------------------------------------------------------------------------- //
    private void Start()
    {
        doorTransform = GetComponentInChildren<Transform>();

        initialRotation = doorTransform.localEulerAngles;

        interact.SetActive(false);
    }

    private void Update()
    {
        CheckForKey();

        if (inRange && isKey && Input.GetKeyDown(KeyCode.F))
        {
            isOpen = !isOpen;
        }

        OpenDoor();
    }

// ---------------------------------------------------------------------------- //

    private void OpenDoor()
    {
        targetRotation = isOpen ? maxRotationAngle : 0f;

        currentRotation = Mathf.MoveTowards(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 newRotation = initialRotation;
        newRotation.y += currentRotation;
        doorTransform.localEulerAngles = newRotation;
    }

    private void CheckForKey()
    {
        isKey = GetComponent<Items>().key;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            interact.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            interact.SetActive(false);
        }
    }
}

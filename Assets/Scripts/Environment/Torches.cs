using UnityEngine;
using UnityEngine.UI;

public class Torches : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject lightSource;
    [SerializeField] private GameObject torchObject;
    [SerializeField] private GameObject button;
    [SerializeField] private Material torchMaterial;

    [Header("Color")]
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor = Color.black;

    [Header("Debug")]
    [SerializeField] private bool inRange;
    [SerializeField] private bool isOn = true;


// -------------------------------------------------------- //

    private void Start()
    {
        lightSource.SetActive(true);
        //button.SetActive(false);

        Renderer renderer = torchObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Create instance of the material to avoid changing the original
            torchMaterial = new Material(renderer.material);
            renderer.material = torchMaterial;
        }

        // Enable emission and set initial color
        torchMaterial.EnableKeyword("_EMISSION");
        torchMaterial.SetColor("_EmissionColor", onColor);
    }


    private void Update()
    {
        if (inRange)
        {
            //button.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleLights();
            }
        }
        else
        {
            //button.SetActive(false);
        }
    }


// -------------------------------------------------------- //

    private void ToggleLights()
    {
        isOn = !isOn;
        lightSource.SetActive(isOn);

        UpdateColor();
    }

    private void UpdateColor()
    {
        if (torchMaterial != null)
        {
            Color newColor = isOn ? onColor : offColor;
            torchMaterial.SetColor("_EmissionColor", newColor);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;
        }
    }

        private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = false;
        }
    }


}
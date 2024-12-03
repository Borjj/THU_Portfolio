using UnityEngine;
using UnityEngine.UI;

public class PlayerLight : MonoBehaviour 
{
    public Light pointLight;
    public Material torchMaterial;
    public float lightIntensity = 2f;
    public LayerMask clueLayer;

    private bool isLightOn;

// ---------------------------------------------------------------------------------------- //
    private void Start()
    {
        pointLight.intensity = 0f;
        isLightOn = false;
        ToggleEmission(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isLightOn = !isLightOn;
            ToggleEmission(true);
            pointLight.intensity = isLightOn ? lightIntensity : 0f;

            if (!isLightOn)
            {
                HideAllClues();
                ToggleEmission(false);
            }
        }

        if (isLightOn)
        {
            CheckForClues();
        }

    }

// -------------------------------------------------------------------------------------- //

    void CheckForClues()
    {    
        // Hide all clues first
        HideAllClues();

        // Show only Clues in range
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, pointLight.range, clueLayer);

        foreach (Collider collider in nearbyColliders)
        {
            Clue clue = collider.GetComponent<Clue>();
            if (clue != null)
            {
                clue.Reveal();
            }
        }
    }

    void HideAllClues()
    {
        Clue[] allClues = FindObjectsOfType<Clue>();
        foreach (Clue clue in allClues)
        {
            clue.Hide();
        }
    }

    void ToggleEmission (bool enable)
    {
        if (enable)
        {
            torchMaterial.EnableKeyword("_EMISSION");
        }
        else
        {
            torchMaterial.DisableKeyword("_EMISSION");
        }
    }

    void OnDrawGizmos()
    {
        if (pointLight != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pointLight.range);
        }
    }
}
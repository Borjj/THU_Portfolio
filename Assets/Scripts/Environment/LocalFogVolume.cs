using UnityEngine;
using UnityEngine.Rendering;

public class LocalFogVolume : MonoBehaviour
{
    [Header("Fog Settings")]
    public Color fogColor = Color.white;
    public float fogDensity = 0.1f;
    public bool useLinearFog = true;
    public float fogStartDistance = 0f;
    public float fogEndDistance = 100f;

    private Color originalFogColor;
    private float originalFogDensity;
    private bool originalFogEnabled;
    private FogMode originalFogMode;
    private float originalStartDistance;
    private float originalEndDistance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) /*|| other.CompareTag("MainCamera")*/
        {
            // Store original fog settings
            originalFogEnabled = RenderSettings.fog;
            originalFogColor = RenderSettings.fogColor;
            originalFogDensity = RenderSettings.fogDensity;
            originalFogMode = RenderSettings.fogMode;
            originalStartDistance = RenderSettings.fogStartDistance;
            originalEndDistance = RenderSettings.fogEndDistance;

            // Apply new fog settings
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogMode = useLinearFog ? FogMode.Linear : FogMode.Exponential;
            
            if (useLinearFog)
            {
                RenderSettings.fogStartDistance = fogStartDistance;
                RenderSettings.fogEndDistance = fogEndDistance;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            // Restore original fog settings
            RenderSettings.fog = originalFogEnabled;
            RenderSettings.fogColor = originalFogColor;
            RenderSettings.fogDensity = originalFogDensity;
            RenderSettings.fogMode = originalFogMode;
            RenderSettings.fogStartDistance = originalStartDistance;
            RenderSettings.fogEndDistance = originalEndDistance;
        }
    }
}
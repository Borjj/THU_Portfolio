using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    public Image healthBar;
    
    [Header("Respawn Settings")]
    public Transform respawnPoint;
    public float respawnDelay = 2f;
    
    [Header("Debug Settings")]
    public bool showDebugLogs = true;

    private bool isDead = false;
    private PlayerController playerController;
    private CharacterController characterController;

    public Action OnPlayerDeath;
    public Action OnPlayerRespawn;



// -------------------------------------------------------------------------- //

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
        UpdateHealthBar();

        if (respawnPoint == null)
        {
            Debug.LogWarning("No respawn point assigned to PlayerHealth script!");
        }
    }


// ---------------------------------------------------------------------------- //

    public void TakeDamage(float damage)
    {
        if (isDead) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthBar();

        if (showDebugLogs)
        {
            Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");
        }
        
        if (currentHealth <= 0 && !isDead)
        {
            if (showDebugLogs)
            {
                Debug.Log("Player health reached 0 - Initiating death sequence");
            }
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;
        
        float previousHealth = currentHealth;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        
        if (showDebugLogs)
        {
            Debug.Log($"Player healed for {currentHealth - previousHealth}. Current health: {currentHealth}");
        }
        
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        if (isDead) return; // Prevent multiple death calls
        
        isDead = true;
        
        if (showDebugLogs)
        {
            Debug.Log("Player died - Disabling controls and preparing respawn");
        }
        
        // Disable player controls
        if (playerController != null)
        {
            playerController.enabled = false;
        }
        
        // Disable character controller to prevent physics interactions
        if (characterController != null)
        {
            characterController.enabled = false;
        }
        
        // Notify other scripts of player death
        OnPlayerDeath?.Invoke();
        
        // Start respawn sequence
        Invoke(nameof(Respawn), respawnDelay);
    }

    void Respawn()
    {
        if (!isDead) return; // Prevent respawn if not dead
        
        if (showDebugLogs)
        {
            Debug.Log("Respawning player");
        }
        
        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthBar();
        
        if (respawnPoint != null)
        {
            // Disable character controller before teleporting
            if (characterController != null)
            {
                characterController.enabled = false;
                transform.position = respawnPoint.position;
                characterController.enabled = true;
            }
            else
            {
                transform.position = respawnPoint.position;
            }
        }
        else
        {
            Debug.LogError("No respawn point assigned! Player will respawn at current position.");
        }
        
        // Re-enable player controls
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        
        // Notify other scripts of respawn
        OnPlayerRespawn?.Invoke();
    }

    // Public method to check player's death state
    public bool IsDead()
    {
        return isDead;
    }
    
    // Public method to get current health percentage
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}

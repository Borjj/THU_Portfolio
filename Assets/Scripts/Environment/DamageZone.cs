using UnityEngine;
using UnityEngine.UI;


public class DamageZone : MonoBehaviour
{
    [Header("Damage Settings")]
    public float damagePerSecond = 20f;
    public bool isConstantDamage = true;
    public float damageInterval = 0.5f;
    
    private float nextDamageTime;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                if (isConstantDamage)
                {
                    playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
                }
                else if (Time.time >= nextDamageTime)
                {
                    playerHealth.TakeDamage(damagePerSecond * damageInterval);
                    nextDamageTime = Time.time + damageInterval;
                }
            }
        }
    }
}
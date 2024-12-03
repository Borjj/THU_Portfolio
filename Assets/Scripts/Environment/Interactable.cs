using UnityEngine;
using UnityEngine.UI; // For UI elements

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameObject button;
    [SerializeField] private Animator anim;
    [SerializeField] private bool hasInteracted = false;
    [SerializeField] private bool inRange;
    
    // Cast bar related variables
    [Header("Cast Bar Settings")]
    [SerializeField] private float castTime = 2f;
    [SerializeField] private Image castBarFill;
    [SerializeField] private GameObject castBarObject;
    
    private bool isCasting = false;
    private float currentCastTime = 0f;
    private Vector3 castStartPosition;


// ----------------------------------------------------------------------------------------- //    
    private void Start()
    {
        button.SetActive(false);
    }
    
    private void Update()
    {
        if (inRange)
        {
            button.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.F) && !isCasting && !hasInteracted)
            {
                StartCasting();
            }
        }
        else
        {
            button.SetActive(false);
            if (isCasting)
            {
                InterruptCast();
            }
        }
        
        if (isCasting)
        {
            UpdateCasting();
        }
    }

// ----------------------------------------------------------------------------------------- //

    
    private void StartCasting()
    {
        isCasting = true;
        currentCastTime = 0f;
        castBarObject.SetActive(true);
        castStartPosition = transform.position; // Store position when cast starts
    }
    
    private void UpdateCasting()
    {
        // Check if player has moved from cast start position
        if (Vector3.Distance(transform.position, castStartPosition) > 0.1f)
        {
            InterruptCast();
            return;
        }
        
        currentCastTime += Time.deltaTime;
        castBarFill.fillAmount = currentCastTime / castTime;
        
        if (currentCastTime >= castTime)
        {
            CompleteCast();
        }
    }
    
    private void CompleteCast()
    {
        isCasting = false;
        castBarObject.SetActive(false);
        hasInteracted = true;
        MovePillar();
    }
    
    private void InterruptCast()
    {
        isCasting = false;
        currentCastTime = 0f;
        castBarObject.SetActive(false);
        castBarFill.fillAmount = 0f;
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
    
    private void MovePillar()
    {
        anim.SetTrigger("Play");
    }
}
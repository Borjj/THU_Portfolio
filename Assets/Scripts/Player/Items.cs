using UnityEngine;

public class Items : MonoBehaviour
{
    [Header ("Weapons")]
    public GameObject weapon2H;
    public GameObject weapon1H;
    [SerializeField] private Transform backSlot;
    [SerializeField] private Transform rightSlot;
    [SerializeField] private Transform handSlot;

    [Header ("Items")]
    public bool key;
    private bool keyRange = false;

    [Header ("UI")]
    [SerializeField] private GameObject interact;
    // --------------------------------------------------------------------- //
    private void Start()
    {
        key = false;
    }
    void Update()
    {
        if (keyRange && Input.GetKeyDown(KeyCode.F))
        {
            key = true;
        }
    }

    // ----------------------------------------------------------------------- //

    void DrawWeapon()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key"))
        {
            keyRange = true;
            interact.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Key"))
        {  
            keyRange = false;
            interact.SetActive(false);
        }
    }
}

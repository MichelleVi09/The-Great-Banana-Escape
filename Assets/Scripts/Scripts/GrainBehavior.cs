using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GrainBehavior : MonoBehaviour
{
    [Header("Grain Settings")]
    [SerializeField] private ItemClass grainItem; //assign grain asset
    [SerializeField] private int quantity = 1;

    [Header("Optional")]
    [SerializeField] private AudioClip pickupSound;

    private void Reset()
    {
        //ensuring setup
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        if (!TryGetComponent<Rigidbody>(out var rb))
            rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //find the player's inventory on the thing that entered the trigger
        var playerInv = other.GetComponent<PlayerInventory>();
        if (playerInv == null) return;

        if (grainItem == null)
        {
            Debug.LogWarning("GrainBehavior: grainItem is not assigned.");
            return;
        }

        playerInv.Collect(grainItem, quantity);

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject); //remove the item
    }
}


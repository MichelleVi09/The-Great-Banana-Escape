using UnityEngine;

public class RespawnOnCarHit : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private string carTag = "Car";
    [SerializeField] private AudioClip hitSound;


    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)

    {
        // If we hit a car, teleport this player to the respawn point
        if (other.CompareTag(carTag))
        {
            transform.position = respawnPoint.position;
            Debug.Log("Player hit");
            if (hitSound != null)
                audioSource.PlayOneShot(hitSound);

        }
    }
}



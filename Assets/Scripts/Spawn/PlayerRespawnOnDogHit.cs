using UnityEngine;

public class PlayerRespawnOnDogHit : MonoBehaviour
{
    [Header("Respawn Settings")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private AudioClip hitSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Respawn()
    {
        if (respawnPoint == null)
        {
            Debug.LogWarning("PlayerRespawnOnDogHit: respawnPoint is not assigned!");
            return;
        }

        // Teleport the player
        transform.position = respawnPoint.position;

        // Play hit sound if available
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
public class RespawnOnCarHit : MonoBehaviour
{
    [SerializeField] private int Respawn;
    [SerializeField] private AudioClip hitSound;


    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)

    {
        // If we hit a car, teleport this player to the respawn point
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(Respawn);
            Debug.Log("Player hit");
            //audioSource.Play();
        }


    }
}




using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarpetInteractPressurePlate : MonoBehaviour
{
    [Header("Activation")]
    [SerializeField] string nextScene = "Outside scene";
    [SerializeField] string playerTag = "Player";


    void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Carpet trigger: {other.name} tag={other.tag}");
        if (other.CompareTag(playerTag) && !string.IsNullOrEmpty(nextScene))
            SceneManager.LoadScene(nextScene);
    }


}

using System;
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
        if (!other.CompareTag(playerTag)) return;

        if (!AllCoinsCollectedInScene())
        {
            Debug.Log("Pressure plate locked");
            return;
        }
        if (!string.IsNullOrEmpty(nextScene) )
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    private bool AllCoinsCollectedInScene()
    {
        CoinBehavior[] remainingCoins = FindObjectsOfType<CoinBehavior>();

        //not done if coins are active
        return remainingCoins.Length == 0;
    }
}

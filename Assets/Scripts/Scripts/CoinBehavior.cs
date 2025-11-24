using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;


[RequireComponent(typeof(Collider))]
public class CoinBehavior : MonoBehaviour, IDataPersistence
{
    [Header("Coin Settings")]
    [SerializeField] private int value = 1;

    [Header("Optional Settings")]
    [SerializeField] private AudioClip pickupSound;

    [SerializeField] private string id; //unique id for coin



    [ContextMenu("Generate guid for id")]
    //generate random id's for coins 
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();

    }

    //if no guid generated, will generate one
    private void OnValidate()
    {
        if (string.IsNullOrWhiteSpace(id)) GenerateGuid();
    }

    private void OnEnable()
    {
        //hide immediately if coin was already collected
        if (CoinManager.instance != null && CoinManager.instance.IsCollected(id))
            gameObject.SetActive(false);
    }
    private void Reset()
    {
        //ensuring setup
        var col = GetComponent<Collider>();

        if (!TryGetComponent<Rigidbody>(out var rb))
            rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))  return;

        //tells manager to upadte total and collected ids
        CoinManager.instance.MarkCollected(id, value);

        if (pickupSound) AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        gameObject.SetActive(false);

    }

    public void LoadData(GameData data)
    {
        if (data.coinsCollected != null && data.coinsCollected.Contains(id))
            gameObject.SetActive(false);
    }
    public void SaveData(GameData data) { }
}

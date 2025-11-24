using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinManager : MonoBehaviour, IDataPersistence
{
    public static CoinManager instance { get; private set; }

    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private int baseCoinValue = 1;
    [SerializeField] private float multiplier = 1f;

    public int total { get; private set; }
    private HashSet<string> collectedIds = new HashSet<string>();

    public bool IsCollected(string id) => collectedIds.Contains(id);

    public void MarkCollected(string id, int units)
    {
        if (!collectedIds.Add(id)) return;
        total += Mathf.RoundToInt(units * baseCoinValue * multiplier);
        UpdateUI();
        DataPersistenceManager.instance.SaveGame();
    }

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        UpdateUI();
    }




    private void UpdateUI()
    {
        if (coinText) coinText.text = $"{total}"; 
    }

    public void LoadData(GameData data) 
    {
        total = data.coinCount;
        collectedIds = new HashSet<string>(data.coinsCollected ?? new List<string>());
        UpdateUI();
    }
    public void SaveData(GameData data) 
    {
        data.coinCount = total;
        data.coinsCollected = new List<string>(collectedIds);
    }

}

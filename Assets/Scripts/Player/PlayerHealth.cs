using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private HealthBar healthBar;
    private void Start()
    {
        if (healthBar)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
    }
    private void Awake()
    {
        if (currentHealth <= 0) currentHealth = maxHealth;
        UpdateUI();
    }
    public void DamagePlayer(int amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
        UpdateUI();
        if (currentHealth ==0)
        {
            //TODO handle death
            Debug.Log("Player died");
        }
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (!healthBar) return;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
    }

    public void LoadData(GameData data)
    {
        maxHealth = data.playerMaxHealth;
        currentHealth = Mathf.Clamp(data.playerCurrentHealth, 0, maxHealth);
        UpdateUI();

    }
    public void SaveData(GameData data) 
    {
        data.playerMaxHealth = maxHealth;
        data.playerCurrentHealth = currentHealth;
    }
}

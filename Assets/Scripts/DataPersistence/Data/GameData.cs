using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{
    public string lastSceneName;
    public int coinCount;
    public Vector3 playerPosition;
    public bool hasWorldPosition;
    public List<string> coinsCollected = new List<string>();
    public int playerMaxHealth;
    public int playerCurrentHealth;

    public GameData()
    {
        lastSceneName = "StartScene";
        playerPosition = Vector3.zero;
        coinCount = 0;
        playerMaxHealth = 100;
        playerCurrentHealth = 100;

    }
   
}

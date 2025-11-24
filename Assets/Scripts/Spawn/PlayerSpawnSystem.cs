using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerSpawnSystem : MonoBehaviour, IDataPersistence
{
    [SerializeField] GameObject playerPrefab;
    private bool havePos;
    private Vector3 pos;

    public void LoadData(GameData d)
    {
        havePos = d.hasWorldPosition && d.lastSceneName == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        pos = d.playerPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        var player = GameObject.FindWithTag("Player")?.gameObject
                    ?? Instantiate(playerPrefab);
        player.transform.position = havePos ? pos : transform.position;
    }

    public void SaveData(GameData d)
    {
        var p = GameObject.FindWithTag("Player");
        d.playerPosition = p.transform.position;
        d.hasWorldPosition = true;
        d.lastSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }


}

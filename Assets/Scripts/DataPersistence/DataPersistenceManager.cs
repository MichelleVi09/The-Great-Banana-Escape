using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private bool initializeDataIfNull = false; 
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    //[SerializeField] private bool useEncryption;

    private GameData gameData;
    private bool isNewGamePending = false;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        //gives operating system standard directory for persisting data in a unity project
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }



    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        if (isNewGamePending) 
        {
            foreach(var dpo in dataPersistenceObjects)
            {
                dpo.LoadData(gameData);
            }
            isNewGamePending = false;
            return;
        }
        LoadGame();
    }





    public void NewGame()
    {
        gameData = new GameData();
        isNewGamePending = true;
        dataHandler.Save(gameData);
    }




    public void LoadGame()
    {
        //load any saved data from a ile using the data handler
        //if it doesn't exist, will return null
        this.gameData = dataHandler.Load();

        //start a new game i fthe data is null and we're configured to initilize data for debugging purposes
        if (this.gameData == null && initializeDataIfNull)
        {
            NewGame();
        }
        //if no data can be laoded, dont continue
        if (this.gameData == null)
        {
            Debug.Log("No data was found. A new game needs to be started before data can be loaded");
            return;
        }
        foreach (var dpo in dataPersistenceObjects)
        {
            dpo.LoadData(gameData);
        }

        //push the loaded data to all other scripts that need it
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects) 
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("loaded coin count = " + gameData.coinCount);
    }




    public void SaveGame()
    {
        //if we dont have any game data to save, log a warning 
        if (this.gameData == null)
        {
            Debug.LogWarning("No data was found.A new game nees to be started before data can be saved");
            return;
        }
        //pass the data to other scripts so they can update it 
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        Debug.Log("Saved coin count = " + gameData.coinCount);

        //Save that data to a file using the data handler
        dataHandler.Save(gameData);



    }




    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);

    }
    public bool HasGameData()
    {
        return gameData != null;
    }

}

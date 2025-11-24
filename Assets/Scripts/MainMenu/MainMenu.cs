using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main MenuButtons ")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button controlsButton;

    private void Start()
    {
        if (!DataPersistenceManager.instance.HasGameData())
        {
            continueButton.interactable = false;
        }
    }
    public void OnNewGameClicked()
    {
        DisableMenuButtons();
        //create a new game which will initialize our game data 
        DataPersistenceManager.instance.NewGame();
        //save the game anytime before loading a new sceneD
        DataPersistenceManager.instance.SaveGame();

        //load the gameplay scene which will save the game because of 
        //OnSceneUnloaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("StartScene");
    }
    public void OnContinueGameClicked()
    {
        DisableMenuButtons();
        //load the next scene which will in turn laod the game because of 
        //OnSceneLoaded() in the DataPersistenceManager
        SceneManager.LoadSceneAsync("StartScene");
    }

    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        continueButton.interactable = false;
        controlsButton.interactable = false;
    }
    public void OnControlsButtonClicked()
    {
        DisableMenuButtons();
        SceneManager.LoadSceneAsync("ControlsScene");
    }

}

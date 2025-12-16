using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnButtonQuit : MonoBehaviour
{
    [SerializeField] private string mainMenuName = "Main Menu";
    // Start is called before the first frame update
  

   public void GoToMainMenu()
    {
        DataPersistenceManager.instance.SaveGame();
        SceneManager.LoadScene(mainMenuName);
    }
}

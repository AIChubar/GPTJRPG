using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField] 
    private Button generateNewWorldButton;
 
    [SerializeField] 
    private Button quiteButton;
    public void OnPlayGameClicked()
    {
        //ButtonClickedSound();
        //DataPersistenceManager.instance.NewGame();
        SceneController.LoadScene(1, 1, 1, 0.2f);
        DisableMenuButtons();
    }
    
    public void GenerateNewWorldClicked()
    {
        //ButtonClickedSound();
        //DataPersistenceManager.instance.LoadGame();
        DisableMenuButtons();
    }

    public void OnQuitGameClicked()
    {
        //ButtonClickedSound();
        Application.Quit();
    }
    
    private void DisableMenuButtons()
    {
        newGameButton.interactable = false;
        generateNewWorldButton.interactable = false;
        quiteButton.interactable = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button newGameButton;

    [SerializeField] 
    private Button worldMenuButton;
 
    [SerializeField] 
    private Button quiteButton;

    private TextMeshProUGUI _newGameText;

    void Start()
    {
        _newGameText = newGameButton.GetComponentInChildren<TextMeshProUGUI>();
        CheckNewGame();
    }
    public void OnPlayGameClicked()
    {
        //ButtonClickedSound();
        //DataPersistenceManager.instance.NewGame();
        SceneController.LoadScene(1, 1, 1, 0.2f);
        DisableMenuButtons();
    }
    
    public void CheckNewGame()
        {
            if (WorldManager.worldManager.currentWorld == null)
            {
                newGameButton.gameObject.GetComponent<Button>().interactable = false;
                _newGameText.color = new Color(1, 1, 1, 0.4f);
            }
            else
            {
                newGameButton.gameObject.GetComponent<Button>().interactable = true;
                _newGameText.color = new Color(1, 1, 1, 1f);
            }
        }
    
    public void WClicked()
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
        worldMenuButton.interactable = false;
        quiteButton.interactable = false;
    }
}

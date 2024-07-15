using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that controls the GameMessageMenu UI element. Used when the game starts and after the game is finished.
/// </summary>
public class GameMessageController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    
    [SerializeField] private Button mainMenuButton;
    
    [SerializeField] private TextMeshProUGUI messageText;
    
    [SerializeField] private TextMeshProUGUI messageTitle;

    /// <summary>
    /// Set the text and selects the button for the GameMessageMenu.
    /// </summary>
    /// <param name="isGameOver">If true, sets up the button to return to Main Menu. Otherwise </param>
    /// <param name="gameMessage">Text of the message to be displayed.</param>
    public void SetGameMessage(bool isGameOver, string gameMessage)
    {
        startButton.gameObject.SetActive(!isGameOver);
        mainMenuButton.gameObject.SetActive(isGameOver);
        messageText.text = gameMessage;
        messageTitle.text = isGameOver ? "Game Over" : "Game Starts";
    }
 
}

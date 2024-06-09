using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMessageController : MonoBehaviour
{
    [SerializeField] private Button startButton;
    
    [SerializeField] private Button mainMenuButton;
    
    [SerializeField] private TextMeshProUGUI messageText;
    
    [SerializeField] private TextMeshProUGUI messageTitle;

    public void SetGameMessage(bool isGameOver, string gameMessage)
    {
        startButton.gameObject.SetActive(!isGameOver);
        mainMenuButton.gameObject.SetActive(isGameOver);
        messageText.text = gameMessage;
        messageTitle.text = isGameOver ? "Game Over" : "Game Starts";
    }
 
}

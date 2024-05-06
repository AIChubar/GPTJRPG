using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public enum UIState { WIN, GAMEOVER, BATTLESTART, ESCPAUSE, UNPAUSED, HEROINFO, GROUPINFO, QUESTMENU }

    [Header("UI Elements")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject questMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject heroInfoMenu;
    [SerializeField] private GameObject groupInfoMenu;
    [SerializeField] private GameObject battleStartMenu;

    [SerializeField] private DialogueHUD battleStartHUD;
    [SerializeField] private DialogueHUD winHUD;
    [SerializeField] private DialogueHUD loseHUD;

    private PlayerInput playerInput;
    private UIState currentState = UIState.UNPAUSED;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Pause.performed += TogglePause;
        pauseCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (currentState is UIState.WIN or UIState.GAMEOVER or UIState.BATTLESTART)
            return;
        currentState = currentState == UIState.UNPAUSED ? UIState.ESCPAUSE : UIState.UNPAUSED;
        UpdateUI();
    }

    private void UpdateUI()
    {
        Time.timeScale = currentState == UIState.UNPAUSED ? 1f : 0f;
        pauseCanvas.SetActive(currentState != UIState.UNPAUSED);
        battleStartMenu.SetActive(currentState == UIState.BATTLESTART);
        pauseMenu.SetActive(currentState == UIState.ESCPAUSE);
        winMenu.SetActive(currentState == UIState.WIN);
        loseMenu.SetActive(currentState == UIState.GAMEOVER);
        heroInfoMenu.SetActive(currentState == UIState.HEROINFO);
        groupInfoMenu.SetActive(currentState == UIState.GROUPINFO);
        questMenu.SetActive(currentState == UIState.QUESTMENU);

    }

    public void ShowBattleStartMenu()
    {
        currentState = UIState.BATTLESTART;
        UpdateUI();
        battleStartHUD.SetHUD(GameManager.gameManager.gameData.enemies, GameManager.gameManager.gameData.enemies.battleStartMonologue);
    }
    
    public void ShowWinMenu()
    {
        currentState = UIState.WIN;
        UpdateUI();
        winHUD.SetHUD(GameManager.gameManager.gameData.enemies, GameManager.gameManager.gameData.enemies.winMonologue);
    }

    public void ShowGameOverMenu()
    {
        currentState = UIState.GAMEOVER;
        UpdateUI();
        loseHUD.SetHUD(GameManager.gameManager.gameData.enemies, GameManager.gameManager.gameData.enemies.lostMonologue);
    }

    public void BeginBattle()
    {
        ResumeGame();
        SceneController.LoadScene(2, 1, 1, 0.2f, true);
    }
    public void ResumeGame()
    {
        currentState = UIState.UNPAUSED;
        UpdateUI();
    }

    public void ShowHeroInfoMenu()
    {
        currentState = UIState.HEROINFO;
        UpdateUI();
    }
    
    public void ShowQuestMenu()
    {
        currentState = UIState.QUESTMENU;
        UpdateUI();
    }
    
    public void ShowPauseMenu()
    {
        currentState = UIState.ESCPAUSE;
        UpdateUI();
    }

    public void ShowGroupInfoMenu()
    {
        currentState = UIState.GROUPINFO;
        UpdateUI();
    }
    
    public void MainMenu()
    {
        Destroy(GameManager.gameManager.gameObject);
        ResumeGame(); 
        SceneController.LoadScene(0); 
    }
}

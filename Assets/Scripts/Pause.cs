using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public enum UIState { GAMEMESSAGE, UNITINTERACTION, ESCPAUSE, UNPAUSED, GROUPINFO, RECRUITSYSTEM , QUESTMENU }

    [Header("UI Elements")]
    [SerializeField] private GameObject pauseCanvas;
    
    [SerializeField] public GameObject questMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject groupInfoMenu;
    [SerializeField] private RecruitSystem recruitSystemMenu;
    [SerializeField] private GameMessageController gameMessageMenu;
    [SerializeField] private GameObject unitInteractionMenu;
    
    [SerializeField] private DialogueHUD unitInteractionHUD;
    private PlayerInput playerInput;
    private UIState _currentState = UIState.UNPAUSED;

    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.Pause.performed += TogglePause;
        pauseCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerInput is not null)
            playerInput.Enable();
    }

    private void OnDisable()
    {
        if (playerInput is not null)
            playerInput.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        if (_currentState is UIState.GAMEMESSAGE  or UIState.UNITINTERACTION or UIState.RECRUITSYSTEM)
            return;
        _currentState = _currentState == UIState.UNPAUSED ? UIState.ESCPAUSE : UIState.UNPAUSED;
        UpdateUI();
    }

    private void UpdateUI()
    {
        Time.timeScale = _currentState == UIState.UNPAUSED ? 1f : 0f;
        pauseCanvas.SetActive(_currentState != UIState.UNPAUSED);
        unitInteractionMenu.SetActive(_currentState == UIState.UNITINTERACTION);
        recruitSystemMenu.gameObject.SetActive(_currentState == UIState.RECRUITSYSTEM);
        pauseMenu.SetActive(_currentState == UIState.ESCPAUSE);
        gameMessageMenu.gameObject.SetActive(_currentState == UIState.GAMEMESSAGE);
        groupInfoMenu.SetActive(_currentState == UIState.GROUPINFO);
        questMenu.SetActive(_currentState == UIState.QUESTMENU);

    }

    public void ShowUnitInteractionMenu(JSONReader.DialogueInfo dialogueInfo, JSONReader.UnitGroup group)
    {
        _currentState = UIState.UNITINTERACTION;
        UpdateUI();
        unitInteractionHUD.SetDialogueHUD(dialogueInfo, group);
    }
    public void ShowGameMessageMenu(bool isGameOver, string gameMessage)
    {
        _currentState = UIState.GAMEMESSAGE;
        UpdateUI();
        gameMessageMenu.SetGameMessage(isGameOver, gameMessage);
    }
    public void ShowRecruitSystemMenu(JSONReader.UnitGroup group)
    {
        _currentState = UIState.RECRUITSYSTEM;
        recruitSystemMenu.SetEnemyGroup(group);
        UpdateUI();
    }
    
    public void BeginBattle()
    {
        ResumeGame();
        SceneController.LoadScene(2, 1, 1, 0.2f, true);
    }
    public void ResumeGame()
    {
        _currentState = UIState.UNPAUSED;
        UpdateUI();
    }

    
    
    public void ShowQuestMenu()
    {
        _currentState = UIState.QUESTMENU;
        UpdateUI();
    }
    
    public void ShowPauseMenu()
    {
        _currentState = UIState.ESCPAUSE;
        UpdateUI();
    }

    public void ShowGroupInfoMenu()
    {
        _currentState = UIState.GROUPINFO;
        UpdateUI();
    }
    
    public void MainMenu()
    {
        Destroy(GameManager.gameManager.gameObject);
        ResumeGame(); 
        SceneController.LoadScene(0); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Script that contains the information about the Game Object it attached, to be shown by <see cref="InfoHUD"/>.
/// </summary>
public class Pause : MonoBehaviour
{
    /// <summary>
    /// State of the Pause Menu.
    /// </summary>
    public enum UIState { GAMEMESSAGE, UNITINTERACTION, ESCPAUSE, UNPAUSED, GROUPINFO, RECRUITSYSTEM , QUESTMENU }

    [SerializeField] private GameObject pauseCanvas;
    [Header("UI Elements")]
    [SerializeField] public QuestMenu questMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject groupInfoMenu;
    [SerializeField] private RecruitSystem recruitSystemMenu;
    [SerializeField] private GameMessageController gameMessageMenu;
    [SerializeField] private GameObject unitInteractionMenu;
    [SerializeField] private DialogueHUD unitInteractionHUD;

    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
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
        if ((_currentState is UIState.GAMEMESSAGE  or UIState.UNITINTERACTION or UIState.RECRUITSYSTEM) || GameManager.gameManager.kbOpened)
            return;
        _currentState = _currentState == UIState.UNPAUSED ? UIState.ESCPAUSE : UIState.UNPAUSED;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (GameManager.gameManager.transitioning)
        {
            _currentState = UIState.UNPAUSED;
        }
        Time.timeScale = _currentState == UIState.UNPAUSED ? 1f : 0f;
        pauseCanvas.SetActive(_currentState != UIState.UNPAUSED);
        unitInteractionMenu.SetActive(_currentState == UIState.UNITINTERACTION);
        recruitSystemMenu.gameObject.SetActive(_currentState == UIState.RECRUITSYSTEM);
        pauseMenu.SetActive(_currentState == UIState.ESCPAUSE);
        gameMessageMenu.gameObject.SetActive(_currentState == UIState.GAMEMESSAGE);
        groupInfoMenu.SetActive(_currentState == UIState.GROUPINFO);
        questMenu.gameObject.SetActive(_currentState == UIState.QUESTMENU);
        GameManager.gameManager.pauseOpened = _currentState != UIState.UNPAUSED;
        GameManager.gameManager.kbButton.interactable = _currentState == UIState.UNPAUSED;
    }
    /// <summary>
    /// Called from the <see cref="WorldEnemy.OnCollisionEnter2D"/> when <see cref="Hero"/> collide with the <see cref="WorldEnemy"/>. Opens up the <see cref="DialogueHUD"/> UI.
    /// </summary>
    public void ShowUnitInteractionMenu(JSONReader.DialogueInfo dialogueInfo, JSONReader.UnitGroup group)
    {
        _currentState = UIState.UNITINTERACTION;
        UpdateUI();
        unitInteractionHUD.SetDialogueHUD(dialogueInfo, group);
    }
    /// <summary>
    /// Opens up the <see cref="GameMessageController"/> UI.
    /// </summary>
    public void ShowGameMessageMenu(bool isGameOver, string gameMessage)
    {
        GameManager.gameManager.transitioning = false;
        _currentState = UIState.GAMEMESSAGE;
        UpdateUI();
        gameMessageMenu.SetGameMessage(isGameOver, gameMessage);
    }
    /// <summary>
    /// Called from the <see cref="OutcomeButtons.OnClickedFight"/> while inside the <see cref="DialogueHUD"/>. Opens up the <see cref="RecruitSystem"/> UI.
    /// </summary>
    public void ShowRecruitSystemMenu(JSONReader.UnitGroup group)
    {
        _currentState = UIState.RECRUITSYSTEM;
        recruitSystemMenu.SetEnemyGroup(group);
        UpdateUI();
    }
    /// <summary>
    /// Called from the <see cref="OutcomeButtons.OnClickedFight"/> while inside the <see cref="DialogueHUD"/>. Switches to the Battle Scene.
    /// </summary>
    public void BeginBattle()
    {
        ResumeGame();
        SceneController.LoadScene(2, 1f, 1f, 0.9f, true);
    }
    /// <summary>
    /// Called by Button from main Pause UI window. Closes the pause menu.
    /// </summary>
    public void ResumeGame()
    {
        AudioManager.instance.Play(ButtonClick);
        questMenu.UpdateQuestHUD();
        _currentState = UIState.UNPAUSED;
        UpdateUI();
    }

    
    /// <summary>
    /// Called by Button from main Pause UI window. Shows the <see cref="QuestMenu"/>.
    /// </summary>
    public void ShowQuestMenu()
    {
        AudioManager.instance.Play(ButtonClick);

        _currentState = UIState.QUESTMENU;
        UpdateUI();
    }
    /// <summary>
    /// Shows the main Pause UI window.
    /// </summary>
    public void ShowPauseMenu()
    {
        AudioManager.instance.Play(ButtonClick);
        _currentState = UIState.ESCPAUSE;
        UpdateUI();
    }
    /// <summary>
    /// Called by Button from main Pause UI window. Shows the <see cref="GroupInfoHUD"/>.
    /// </summary>
    public void ShowGroupInfoMenu()
    {
        AudioManager.instance.Play(ButtonClick);

        _currentState = UIState.GROUPINFO;
        UpdateUI();
    }
    /// <summary>
    /// Called by Button from main Pause UI window. Returns to the <see cref="MainMenu"/>.
    /// </summary>
    public void MainMenu()
    {
        ResumeGame();
        AudioManager.instance.Play(ButtonClick);
        SceneController.LoadScene(0); 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public enum UIState { WIN, GAMEOVER, ESCPAUSE, UNPAUSED, HEROINFO, GROUPINFO }

    [Header("UI Elements")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject loseMenu;
    [SerializeField] private GameObject heroInfoMenu;
    [SerializeField] private GameObject groupInfoMenu;

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
        currentState = currentState == UIState.UNPAUSED ? UIState.ESCPAUSE : UIState.UNPAUSED;
        UpdateUI();
    }

    private void UpdateUI()
    {
        Time.timeScale = currentState == UIState.UNPAUSED ? 1f : 0f;
        pauseCanvas.SetActive(currentState != UIState.UNPAUSED);

        pauseMenu.SetActive(currentState == UIState.ESCPAUSE);
        winMenu.SetActive(currentState == UIState.WIN);
        loseMenu.SetActive(currentState == UIState.GAMEOVER);
        heroInfoMenu.SetActive(currentState == UIState.HEROINFO);
        groupInfoMenu.SetActive(currentState == UIState.GROUPINFO);
    }

    public void ShowWinMenu()
    {
        currentState = UIState.WIN;
        UpdateUI();
    }

    public void ShowGameOverMenu()
    {
        currentState = UIState.GAMEOVER;
        UpdateUI();
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

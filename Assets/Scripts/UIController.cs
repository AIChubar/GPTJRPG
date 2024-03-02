using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public enum UIState { TEXTUAL, GROUP, UNPAUSED }
    
    private UIState _uiState;

    private UIState _lastPauseState;
    
    [SerializeField] private Button rightButton;

    [SerializeField] private Button leftButton;
    
    [Header("Pause canvas object")]
    [SerializeField]
    private GameObject pauseMenu;
    
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    void Start()
    {
        _lastPauseState = UIState.TEXTUAL;
        _uiState = UIState.UNPAUSED;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_uiState == UIState.UNPAUSED)
        {
            UnSetPause();
        }
        else
        {
            SetPause(_lastPauseState);
        }
        if (playerInput.Player.Pause.triggered)
        {
            if (_uiState == UIState.UNPAUSED)
            {
                _uiState = _lastPauseState;
                
            }
            else
            {
                _uiState = UIState.UNPAUSED;
                
            }
        }
    }
// placeholders if there is more than 2 menus
    public void SwitchLeft()
    {
        _lastPauseState = _lastPauseState == UIState.TEXTUAL ? UIState.GROUP : UIState.TEXTUAL;
        SetPause(_lastPauseState);
    }
    
    public void SwitchRight()
    {
        _lastPauseState = _lastPauseState == UIState.GROUP ? UIState.TEXTUAL : UIState.GROUP;
        SetPause(_lastPauseState);
    }
    
    public void SetPause(UIState mode)
    {
        //AudioManager.instance.PauseSounds(true);

        pauseMenu.SetActive(true);
        bool txt = mode == UIState.TEXTUAL;

        gameObject.transform.GetChild(0).GetChild(0).gameObject.SetActive(txt);
        gameObject.transform.GetChild(0).GetChild(1).gameObject.SetActive(!txt);
        
        Time.timeScale = 0f;
    }

    public void UnSetPause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        //AudioManager.instance.PauseSounds(false);

    }
}

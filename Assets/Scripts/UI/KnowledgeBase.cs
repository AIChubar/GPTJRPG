using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Script that controls the Knowledge Base UI object containing various information about the game.
/// </summary>
public class KnowledgeBase : MonoBehaviour
{
    /// <summary>
    /// State of the Knowledge Base Menu.
    /// </summary>
    public enum KBState { BUTTONMENU, WORLD, HEROINFO, CLASSES, GAMEINFO, UNPAUSED }

    private KBState _currentState = KBState.UNPAUSED; 
    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI heroName;
    [SerializeField]
    private TextMeshProUGUI race;
    [SerializeField]
    private TextMeshProUGUI heroClass;
    [SerializeField]
    private TextMeshProUGUI profession;
    [SerializeField]
    private TextMeshProUGUI backStory;
    
    [SerializeField]
    private TextMeshProUGUI worldName;
    
    [SerializeField]
    private TextMeshProUGUI worldDescription;
    
    [SerializeField]
    private TextMeshProUGUI level1Name;
    
    [SerializeField]
    private TextMeshProUGUI level1Description;
    
    [SerializeField]
    private TextMeshProUGUI level2Name;
    
    [SerializeField]
    private TextMeshProUGUI level2Description;
    
    [SerializeField]
    private TextMeshProUGUI level3Name;
    
    [SerializeField]
    private TextMeshProUGUI level3Description;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    private PlayerInput playerInput;
    
    [SerializeField] private GameObject unitLevelPrefab;
    
    [SerializeField] private Transform unitLevelParent;


    [SerializeField] private GameObject buttonMenu;
    [SerializeField] private GameObject worldMenu;
    [SerializeField] private GameObject heroInfoMenu;
    [SerializeField] private GameObject classesMenu;
    [SerializeField] private GameObject gameInfoMenu;
    private void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.Player.KB.performed += ToggleKB;
        playerInput.Player.Pause.performed += TogglePause;
    }
    // Start is called before the first frame update
    void Start()
    {
        var hero = GameManager.gameManager.hero;
        heroName.text = hero.heroName;
        race.text = hero.heroRace;
        heroClass.text = hero.heroClass;
        profession.text = hero.heroProfession;
        backStory.text = hero.heroBackStory;

        var narrative = GameManager.gameManager.world.narrativeData;
        var levels = GameManager.gameManager.world.levels;

        worldName.text = narrative.worldName;
        worldDescription.text = narrative.story;

        level1Name.text = "Level one: " + levels[0].levelName;
        level1Description.text = levels[0].levelDescription;
        
        level2Name.text = "Level two: " + levels[1].levelName;
        level2Description.text = levels[1].levelDescription;
        
        level3Name.text = "Level three: " + levels[2].levelName;
        level3Description.text = levels[2].levelDescription;
        _currentState = KBState.UNPAUSED;

        var unitsData = GameManager.gameManager.world.unitsData;

        var levelInfo = Instantiate(unitLevelPrefab, unitLevelParent);
        levelInfo.GetComponent<UnitGroupInfo>().SetGroup(GameManager.gameManager.world.mainCharacter.protagonistGroup);
        foreach (var level in unitsData.levelsUnits)
        {
            levelInfo = Instantiate(unitLevelPrefab, unitLevelParent);
            levelInfo.GetComponent<UnitGroupInfo>().SetLevel(level);
        }
        levelInfo = Instantiate(unitLevelPrefab, unitLevelParent);
        levelInfo.GetComponent<UnitGroupInfo>().SetGroup(GameManager.gameManager.world.narrativeData.antagonistGroup);
        UpdateUI();
    }

    private void TogglePause(InputAction.CallbackContext obj)
    {
        if (_currentState == KBState.UNPAUSED)
            return;
        ShowKnowledgeBase();
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
    private void UpdateUI()
    {
        
        if (GameManager.gameManager.transitioning)
        {
            _currentState = KBState.UNPAUSED;
        }
        Time.timeScale = _currentState == KBState.UNPAUSED ? 1f : 0f;
        buttonMenu.SetActive(_currentState == KBState.BUTTONMENU);
        worldMenu.SetActive(_currentState == KBState.WORLD);
        heroInfoMenu.gameObject.SetActive(_currentState == KBState.HEROINFO);
        classesMenu.SetActive(_currentState == KBState.CLASSES);
        gameInfoMenu.gameObject.SetActive(_currentState == KBState.GAMEINFO);
        GameManager.gameManager.kbOpened = _currentState != KBState.UNPAUSED;

    }
    
    /// <summary>
    /// Called by Button from main Knowledge Base UI window. Shows the information about the Protagonist.
    /// </summary>
    public void ShowHeroInfoMenu()
    {
        AudioManager.instance.Play(ButtonClick);
        _currentState = KBState.HEROINFO;
        UpdateUI();
    }
    
    /// <summary>
    /// Called by Button from main Knowledge Base UI window. Shows the general information about the current Game World.
    /// </summary>
    public void ShowWorldMenu()
    {
        AudioManager.instance.Play(ButtonClick);

        _currentState = KBState.WORLD;
        UpdateUI();
    }
    /// <summary>
    /// Called by Button from main Knowledge Base UI window. Shows the information about the <see cref="Unit"/> Classes and their abilities.
    /// </summary>
    public void ShowClassesMenu()
    {
        AudioManager.instance.Play(ButtonClick);

        _currentState = KBState.CLASSES;
        UpdateUI();
    }
    
    /// <summary>
    /// Called by Button from main Knowledge Base UI window. Shows the Bestiary with the information about all the <see cref="JSONReader.UnitJSON"/> in the Game.
    /// </summary>
    public void ShowGameInfoMenu()
    {
        AudioManager.instance.Play(ButtonClick);

        _currentState = KBState.GAMEINFO;
        UpdateUI();
    }

    /// <summary>
    /// Called by the button that is always available on the screen. Calls <see cref="ShowKnowledgeBase"/>.
    /// </summary>
    public void OnClickedKnowledgeBase()
    {
        ShowKnowledgeBase();
        AudioManager.instance.Play(ButtonClick);
    }
    /// <summary>
    /// Shows the main Knowledge Base UI window.
    /// </summary>
    public void ShowKnowledgeBase()
    {
        _currentState = _currentState switch
        {
            KBState.WORLD or KBState.GAMEINFO or KBState.HEROINFO or KBState.CLASSES or KBState.UNPAUSED => KBState
                .BUTTONMENU,
            KBState.BUTTONMENU => KBState.UNPAUSED,
            _ => _currentState
        };
        
        UpdateUI();
    }
    private void ToggleKB(InputAction.CallbackContext context)
    {
        if (GameManager.gameManager.pauseOpened)
        {
            return;
        }
        ShowKnowledgeBase();
    }
    
    
}


using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnowledgeBase : MonoBehaviour
{
    public enum KBState { BUTTONMENU, WORLD, HEROINFO, CLASSES, GAMEINFO, UNPAUSED }

    private KBState _currentState = KBState.UNPAUSED; 
    
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


    private PlayerInput playerInput;

    [SerializeField] private GameObject buttonMenu;
    [SerializeField] private GameObject worldMenu;
    [SerializeField] private GameObject heroInfoMenu;
    [SerializeField] private GameObject classesMenu;
    [SerializeField] private GameObject gameInfoMenu;
    // Start is called before the first frame update
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Player.KB.performed += ToggleKB;
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

        level1Name.text += levels[0].levelName;
        level1Description.text = levels[0].levelDescription;
        
        level2Name.text += levels[1].levelName;
        level2Description.text = levels[1].levelDescription;
        
        level3Name.text += levels[2].levelName;
        level3Description.text = levels[2].levelDescription;

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
        Time.timeScale = _currentState == KBState.UNPAUSED ? 1f : 0f;
        buttonMenu.SetActive(_currentState != KBState.UNPAUSED);
        worldMenu.SetActive(_currentState == KBState.WORLD);
        heroInfoMenu.gameObject.SetActive(_currentState == KBState.HEROINFO);
        classesMenu.SetActive(_currentState == KBState.CLASSES);
        gameInfoMenu.gameObject.SetActive(_currentState == KBState.GAMEINFO);


    }
    
    public void ShowHeroInfoMenu()
    {
        _currentState = KBState.HEROINFO;
        UpdateUI();
    }
    
    public void ShowWorldMenu()
    {
        _currentState = KBState.WORLD;
        UpdateUI();
    }
    
    public void ShowClassesMenu()
    {
        _currentState = KBState.CLASSES;
        UpdateUI();
    }
    
    public void ShowGameInfoMenu()
    {
        _currentState = KBState.GAMEINFO;
        UpdateUI();
    }
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
        ShowKnowledgeBase();
    }
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
}

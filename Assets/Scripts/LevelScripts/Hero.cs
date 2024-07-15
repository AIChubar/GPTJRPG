using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
/// <summary>
/// Script that manages Protagonist Hero Object controls and logic.
/// </summary>
public class Hero : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3.0f;

    /// <summary>
    /// Protagonist Group.
    /// </summary>
    [HideInInspector] public JSONReader.UnitGroup allyGroup;

    [HideInInspector]
    public string heroName;
    
    [HideInInspector]
    public string heroRace;
    
    [HideInInspector]
    public string heroClass;
    
    [HideInInspector]
    public string heroProfession;
    
    [HideInInspector]
    public string heroBackStory;
    
    private Rigidbody2D rb;
    
    private PlayerInput playerInput;

    [HideInInspector] public int amuletOfHealing = 0;
    
    [HideInInspector] public int amuletOfAlliance = 0;
    

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = gameObject.GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        allyGroup = GameManager.gameManager.world.mainCharacter.protagonistGroup;
        
        
        heroName = GameManager.gameManager.world.mainCharacter.name;
        heroRace = GameManager.gameManager.world.mainCharacter.race;
        heroClass = GameManager.gameManager.world.mainCharacter.characterClass;
        heroProfession = GameManager.gameManager.world.mainCharacter.occupation;
        heroBackStory = GameManager.gameManager.world.mainCharacter.backStory;
        GameEvents.gameEvents.OnUnitKilled += GameEvents_OnUnitKilled;
    }

    private void GameEvents_OnUnitKilled(Unit unit)
    {
        //allyGroup.units.(unit.unitInfo);
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
    
    
    void FixedUpdate()
    {
        if (GameManager.gameManager is not null && !GameManager.gameManager.transitioning && playerInput is not null)
        {
            var movement = playerInput.Player.Move.ReadValue<Vector2>();
            rb.MovePosition(rb.position + movement * (movementSpeed * Time.fixedDeltaTime));
        }
    }
}

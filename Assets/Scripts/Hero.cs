using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // public Animator animator;

    public bool movementsEnabled = true;

    [SerializeField] private float movementSpeed = 3.0f;

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

    private void Awake()
    {
        playerInput = new PlayerInput();
        rb = gameObject.GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        allyGroup = GameManager.gameManager.world.unitsData.friendlyGroup;
        
        
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
        if (GameManager.gameManager == null || !GameManager.gameManager.transitioning || playerInput is null)
        {
            var movement = playerInput.Player.Move.ReadValue<Vector2>();
            rb.MovePosition(rb.position + movement * (movementSpeed * Time.fixedDeltaTime));
        }
    }
}

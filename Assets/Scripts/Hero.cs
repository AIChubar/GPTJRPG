using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // public Animator animator;

    public bool movementsEnabled = true;

    [HideInInspector]
    public GroupData allyGroup;

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
        allyGroup = GameManager.gameManager.unitsData.allyGroup;
        heroName = GameManager.gameManager.world.mainCharacter.name;
        heroRace = GameManager.gameManager.world.mainCharacter.race;
        heroClass = GameManager.gameManager.world.mainCharacter.characterClass;
        heroProfession = GameManager.gameManager.world.mainCharacter.occupation;
        heroBackStory = GameManager.gameManager.world.mainCharacter.backStory;

    }
    
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    
    void FixedUpdate()
    {
        if (movementsEnabled)
        {
            var movement = playerInput.Player.Move.ReadValue<Vector2>();
            rb.MovePosition(rb.position + movement * (3.0f * Time.fixedDeltaTime));
        }
    }
}

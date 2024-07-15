using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

/// <summary>
/// Main game Script containing data that contains fields and method used accessed from multiple other scripts.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Current <see cref="BattleJournal"/> for the Battle Scene.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Current BattleJournal for the Battle Scene.")]
    public BattleJournal battleJournal;
    
    /// <summary>
    /// Prefab of the UI object used as the damage number text floating above the damaged <see cref="Unit"/>.
    /// </summary>
    [UnityEngine.Tooltip("Prefab of the UI object used as the damage number text floating above the damaged Unit")]
    public GameObject floatingTextPrefab;

    /// <summary>
    /// True if player is currently in the Battle Scene.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("True if player is currently in the Battle Scene.")]
    public bool inBattle = false;
    
    /// <summary>
    /// Contains units data for the current Battle.
    /// </summary>
    [UnityEngine.Tooltip("Contains units data for the current Battle.")]
    public GameData gameData;
    
    /// <summary>
    /// True if the scene is still transitioning.
    /// </summary>
    [UnityEngine.Tooltip("True if the scene is still transitioning.")]
    public bool transitioning;

    /// <summary>
    /// <see cref="BattleSystem.BattleState"/> of the current Battle.
    /// </summary>
    [UnityEngine.Tooltip("BattleSystem.BattleState of the current Battle.")]
    public BattleSystem.BattleState battleResult = BattleSystem.BattleState.START;

    /// <summary>
    /// <see cref="JSONReader.GameWorld"/> of the current game instance.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("JSONReader.GameWorld of the current game instance.")]
    public JSONReader.GameWorld world;

    /// <summary>
    /// Sprite that is used if there was an error when finding a needed sprite.
    /// </summary>
    [UnityEngine.Tooltip("Sprite that is used if there was an error when finding a needed sprite.")]
    public Sprite errorSprite;
    
    /// <summary>
    /// Sprite of the armour up Button.
    /// </summary>
    [UnityEngine.Tooltip("Sprite of the armour up Button.")]
    public Sprite armouredSprite;
    /// <summary>
    /// Sprite of the attack Button.
    /// </summary>
    [UnityEngine.Tooltip("Sprite of the attack Button.")]
    public Sprite attackSprite;
    
    /// <summary>
    /// Prefab of the GameObject used to indicate who was affected by the <see cref="Unit"/> Ability.
    /// </summary>
    [UnityEngine.Tooltip("Prefab of the GameObject used to indicate who was affected by the Unit Ability.")]
    public GameObject abilityIndicationPrefab;
    
    
    [HideInInspector]
    public int levelIndex = 0;
    
    public Hero hero;

    public Pause pauseMenu;
    /// <summary>
    /// Prefab of the GameObject used to indicate who was affected by the <see cref="Unit"/> Ability.
    /// </summary>
    [UnityEngine.Tooltip("Prefab of the GameObject used to indicate who was affected by the Unit Ability.")]
    public GameObject map;

    /// <summary>
    /// Atlas of all unit sprites, accessed when the <see cref="Unit"/> is created.
    /// </summary>
    [UnityEngine.Tooltip("Atlas of all unit sprites, accessed when the Unit is created.")]
    public SpriteAtlas atlasUnit;
    
    /// <summary>
    /// Atlas of all ability icons.
    /// </summary>
    [UnityEngine.Tooltip("Atlas of all ability icons.")]
    public SpriteAtlas atlasAbility;

    [HideInInspector] public BattleSystem battleSystem;
    
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material defaultMaterial;

    /// <summary>
    /// Main Game Antagonist object.
    /// </summary>
    [HideInInspector] public GameObject villain = null;
    
    /// <summary>
    /// True if the <see cref="KnowledgeBase"/> is active.
    /// </summary>
    [HideInInspector] public bool kbOpened = false;
    /// <summary>
    /// True if the <see cref="Pause"/> is active.
    /// </summary>
    [HideInInspector] public bool pauseOpened = false;
    
    /// <summary>
    /// Button object that opens the <see cref="KnowledgeBase"/>.
    /// </summary>
    [UnityEngine.Tooltip("Button object that opens the KnowledgeBase")]
    public Button kbButton;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound DeathSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound SorcererSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound PaladinSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ProtectorSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound BastionSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound HealerSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound TricksterSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound BerserkerSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound MarksmanSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ShamanSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound AttackSound;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound DefendSound;
   
    /// <summary>
    /// Dictionary that contains descriptions for class abilities.
    /// </summary>
    public Dictionary<Unit.UnitType, UnitClass> classesDescriptions = new()
    {
        [Unit.UnitType.fighter] = new UnitClass("Fighter", "Attacks back when being attacked by the enemy", "Cooldown: passive ability"),
        [Unit.UnitType.sorcerer] = new UnitClass("Sorcerer", "Stuns enemy unit for 1 turn", "Cooldown: 1 turn"),
        [Unit.UnitType.paladin] = new UnitClass("Paladin", "Heals all ally units for 15% of their max health", "Cooldown: 4 turns"),
        [Unit.UnitType.protector] = new UnitClass("Protector", "Taunts all enemy units for 1 turn", "Cooldown: 2 turns"),
        [Unit.UnitType.bastion] = new UnitClass("Bastion", "Activates armour up for all ally units for 2 turns", "Cooldown: 3 turns"),
        [Unit.UnitType.healer] = new UnitClass("Healer", "Heals ally unit for 20% of their max health", "Cooldown: 1 turn"),
        [Unit.UnitType.trickster] = new UnitClass("Trickster", "Makes enemy unit attack another enemy unit the next turn", "Cooldown: 4 turns"),
        [Unit.UnitType.berserker] = new UnitClass("Berserker", "Attacks all enemy units, also hurting oneself", "Cooldown: 3 turns"),
        [Unit.UnitType.marksman] = new UnitClass("Marksman", "Attacks enemy unit ignoring their armour", "Cooldown: 1 turn"),
        [Unit.UnitType.shaman] = new UnitClass("Shaman", "All ally units have 50% to avoid damage for 1 round", "Cooldown: 3 turns") };
    
    public static GameManager gameManager  { get; private set; }
    private void Awake()
    {
        if (gameManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Method called when the battle scene is unloaded in <see cref="SceneController"/>.
    /// </summary>
    public void SceneUnloaded()
    {
        if (map == null || map.Equals(null))
            map = GameObject.FindGameObjectWithTag("Map");
        map.SetActive(true);
        inBattle = false;
        hero.gameObject.SetActive(true);
        gameManager.pauseMenu.questMenu.questHUD.SetActive(true);
        gameManager.pauseMenu.questMenu.UpdateQuestHUD();

        
    }
    /// <summary>
    /// Method called when the scene is unloaded and finished transitioning in <see cref="SceneController"/>.
    /// </summary>
    public void SceneFinishedUnloading()
    {
        if (battleResult == BattleSystem.BattleState.LOST)
        {
            pauseMenu.ShowGameMessageMenu(true, world.narrativeData.gameOverMessage);
        }
        else if (battleResult == BattleSystem.BattleState.WIN)
        {
            if (gameData.isBossFight)
                pauseMenu.ShowGameMessageMenu(true, world.narrativeData.defeatEndingMessage);
        }
        
    }
    
    /// <summary>
    /// Method called when the scene is loaded in <see cref="SceneController"/>.
    /// </summary>
    public void SceneLoaded()
    {
        if (map == null || map.Equals(null))
            map = GameObject.FindGameObjectWithTag("Map");
        if (hero == null || hero.Equals(null))
        {
            hero = GameObject.FindGameObjectWithTag("Hero").GetComponent<Hero>();
        }

        if (pauseMenu != null && !pauseMenu.Equals(null))
        {
            pauseMenu.questMenu.GetComponent<QuestMenu>().SetQuests();
        }
        
    }
    /// <summary>
    /// Method called when the scene is loaded and finished transitioning in <see cref="SceneController"/>.
    /// </summary>
    public void SceneFinishedLoading()
    {
        if (levelIndex == 0)
        {
            pauseMenu.ShowGameMessageMenu(false, world.narrativeData.greetingGameMessage);
        }
    }
    /// <summary>
    /// Method called when the additive scene is loaded in <see cref="SceneController"/>.
    /// </summary>
    public void SceneAdditive()
    {
        inBattle = true;
        
        if (map == null || map.Equals(null))
            map = GameObject.FindGameObjectWithTag("Map");
        hero.gameObject.SetActive(false);
        gameManager.pauseMenu.questMenu.questHUD.SetActive(false);
    }

    /// <summary>
    /// Get unit sprite from <see cref="atlasUnit"/>.
    /// </summary>
    /// <param name="unit"><see cref="Unit"/> for which the sprite should be returned.</param>
    public Sprite GetSpriteUnit(JSONReader.UnitJSON unit)
    {
        if (unit is null)
            return errorSprite;
        var unitNameForSpriteAtlas = unit.secondAttribute;
        if (unit.thirdAttribute != "")
            unitNameForSpriteAtlas += "_" + unit.thirdAttribute;
        var sprite = atlasUnit.GetSprite(unitNameForSpriteAtlas);
        if (sprite is null)
            return errorSprite;
        return sprite;
    }
    
    /// <summary>
    /// Get ability sprite from <see cref="atlasAbility"/>.
    /// </summary>
    /// <param name="unit"><see cref="Unit"/> for which class ability the sprite should be returned.</param>
    public Sprite GetSpriteAbility(JSONReader.UnitJSON unit)
    {
        if (unit is null)
            return errorSprite;
        var sprite = atlasAbility.GetSprite(unit.unitClass);
        if (sprite is null)
            return errorSprite;
        return sprite;
    }
    /// <summary>
    /// Method that adds or removes an outline from the given GameObject.
    /// </summary>
    /// <param name="ob">GameObject to be outlined.</param>
    /// <param name="outlined">If true, outlines, otherwise remove an outline.</param>
    public void OutlineObject(GameObject ob, bool outlined) 
    {
        if (ob)
            ob.GetComponent<Renderer>().material = outlined ? outlineMaterial : defaultMaterial;
    }
    

}

[Serializable]
public class UnitClass
{
    public string className;
    public string abilityDescription;
    public string abilityCooldown;

    public UnitClass(string n, string d, string c)
    {
        className = n;
        abilityDescription = d;
        abilityCooldown = c;
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    
    [HideInInspector]
    public BattleJournal battleJournal;
    
    public GameObject floatingTextPrefab;

    [HideInInspector]
    public bool inBattle = false;
    
    public GameData gameData;
    
    //public UnitsData unitsData;

    
    public bool transitioning = true;

    public BattleSystem.BattleState battleResult = BattleSystem.BattleState.START;

    [HideInInspector]
    public JSONReader.GameWorld world;


    public Sprite errorSprite;
    
    public Sprite armouredSprite;

    public Sprite attackSprite;
    public GameObject abilityIndicationPrefab;
    
    [HideInInspector]
    public int levelIndex = 0;
    
    public Hero hero;

    public Pause pauseMenu;
    
    public GameObject map;

    public SpriteAtlas atlasUnit;
    
    public SpriteAtlas atlasAbility;

    [HideInInspector] public BattleSystem battleSystem;
    
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material defaultMaterial;

    [HideInInspector] public GameObject villain = null;
    
    public bool kbOpened = false;
    public bool pauseOpened = false;
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

    // After fight menuы
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
            if (levelIndex == 2 && villain is not null && !villain.Equals(null))
                villain.SetActive(true);
        }
        
    }
    
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
    
    public void SceneFinishedLoading()
    {
        if (levelIndex == 0)
        {
            pauseMenu.ShowGameMessageMenu(false, world.narrativeData.greetingGameMessage);
        }
    }

    public void SceneAdditive()
    {
        inBattle = true;
        
        if (map == null || map.Equals(null))
            map = GameObject.FindGameObjectWithTag("Map");
        hero.gameObject.SetActive(false);
        gameManager.pauseMenu.questMenu.questHUD.SetActive(false);
    }

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
    
    public Sprite GetSpriteAbility(JSONReader.UnitJSON unit)
    {
        if (unit is null)
            return errorSprite;
        var sprite = atlasAbility.GetSprite(unit.unitClass);
        if (sprite is null)
            return errorSprite;
        return sprite;
    }
    public void OutlineObject(GameObject ob, bool outlined) 
    {
        if (ob)
            ob.GetComponent<Renderer>().material = outlined ? outlineMaterial : defaultMaterial;
    }
    

}

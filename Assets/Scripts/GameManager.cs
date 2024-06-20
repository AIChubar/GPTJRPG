using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    
    [HideInInspector]
    public BattleJournal battleJournal;
    
    public GameObject floatingTextPrefab;

    [HideInInspector]
    public bool inBattle = false;
    
    public GameData gameData;
    
    //public UnitsData unitsData;

    [HideInInspector]
    public bool transitioning = false;

    public BattleSystem.BattleState battleResult = BattleSystem.BattleState.START;

    [HideInInspector]
    public JSONReader.GameWorld world;


    public Sprite errorSprite;
    
    public Sprite armouredSprite;

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

        map.SetActive(false);
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
        var sprite = atlasAbility.GetSprite(unit.unitType);
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

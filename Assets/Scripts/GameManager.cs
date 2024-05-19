using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    public GameObject floatingTextPrefab;
    
    public GameData gameData;
    
    //public UnitsData unitsData;

    [HideInInspector]
    public bool transitioning = false;

    public BattleSystem.BattleState battleResult = BattleSystem.BattleState.START;

    [HideInInspector]
    public JSONReader.GameWorld world;

    
    [HideInInspector]
    public int levelIndex = 0;
    
    public Hero hero;

    public Pause pauseMenu;

    public SpriteAtlas atlas;
    
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material defaultMaterial;

    public GameObject unitHUDGamePrefab;
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


    public void SceneUnloaded()
    {
        if (battleResult == BattleSystem.BattleState.LOST)
        {
            pauseMenu.ShowGameOverMenu();
        }
        else if (battleResult == BattleSystem.BattleState.WIN)
        {
            pauseMenu.ShowWinMenu();
        }
    }


    public Sprite GetSprite(JSONReader.UnitJSON unit)
    {
        var unitNameForSpriteAtlas = unit.secondAttribute;
        if (unit.thirdAttribute != "")
            unitNameForSpriteAtlas += "_" + unit.thirdAttribute;
        return atlas.GetSprite(unitNameForSpriteAtlas);
    }
    public void OutlineObject(GameObject ob, bool outlined) 
    {
        if (ob)
            ob.GetComponent<Renderer>().material = outlined ? outlineMaterial : defaultMaterial;
    }
}

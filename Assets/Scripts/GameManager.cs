using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    public GameData battleData;
    
    public Hero hero;

    public SpriteAtlas atlas;
    
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material defaultMaterial;

    public GameObject unitHUDGamePrefab;
    public static GameManager gameManager  { get; private set; }
    private void Awake()
    {
        /*string path = "Assets/Resources/Monsters.txt";
        Sprite[] sprites =  new Sprite[atlas.spriteCount];
        atlas.GetSprites(sprites);
        StreamWriter writer = new StreamWriter(path, true);

        for (int i = 0; i < sprites.Length; i++)
        {
            writer.WriteLine(sprites[i].name);
        }

        writer.Close();*/
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

    public GameObject MapToBattle(GameObject ob, bool friendly)
    {
        /*var components = ob.GetComponents(typeof(Component))
            .Where(o => o is not (SpriteRenderer or Transform or Collider2D or Rigidbody2D));
 
        foreach(var comp in components)
        {
            Destroy(comp);
        }*/

        var unit = ob.AddComponent<Unit>();
        unit.unitData.friendly = friendly;

        if (!friendly)
        {
            ob.layer = 3;
        }
        return ob;
    }

    public void OutlineObject(GameObject ob, bool outlined) 
    {
        ob.GetComponent<Renderer>().material = outlined ? outlineMaterial : defaultMaterial;
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}

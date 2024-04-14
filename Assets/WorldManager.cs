using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class WorldManager : MonoBehaviour
{
    // Start is called before the first frame update
    /*[HideInInspector]
    public List<WorldButton> _worldButtons = new List<WorldButton>();*/
    [HideInInspector]
    public WorldButton currentWorld;

    public Dictionary<string ,JSONReader.GameWorld> Worlds = new Dictionary<string, JSONReader.GameWorld>();
    
    public GameObject worldButtonPrefab;
    public GameObject worldButtonParent;
    public static WorldManager worldManager  { get; private set; }
    void Awake()
    {
        if (worldManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            worldManager = this;
        }
    }
   

    public void AddButton(string worldName)
    {
        GameObject go = Instantiate(worldButtonPrefab);
        go.GetComponent<WorldButton>().SetButton(worldName);
        go.transform.SetParent(worldButtonParent.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
    }
    
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;

public class WorldManager : MonoBehaviour
{
    private List<string> _units = new List<string>(); 

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

    void Start()
    {
        using StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/API/enemies.txt");
        while (sr.ReadLine() is { } line)
        {
            _units.Add(line);
        }
        
        var info = new DirectoryInfo(Application.streamingAssetsPath + @"/Worlds");
        var fileInfo = info.GetFiles();
        foreach (var file  in fileInfo)
        {
            var fileName = file.Name;
            if (fileName.Contains(".meta"))
                continue;
            var worldName = file.Name.Substring(0, file.Name.Length - 5);
            var jsonWorld =
                JsonUtility.FromJson<JSONReader.GameWorld>(File.ReadAllText(Application.streamingAssetsPath + "/Worlds/" + fileName));
            if (!CheckUnits(jsonWorld))
                continue;
            worldManager.Worlds.Add(worldName, jsonWorld);
            worldManager.AddButton(worldName);
        }
    }
   

    public bool CheckUnits(JSONReader.GameWorld world)
    {
        try
        {
            foreach (var level in world.levels)
            foreach (var group in level.enemyGroups)
            foreach (var enemy in group.units)
                if (!_units.Contains(enemy.unitID))
                {
                    var withoutNum = enemy.unitID.Substring(enemy.unitID.IndexOf('_') + 1);
                    string rightID = _units.FirstOrDefault(stringToCheck => stringToCheck.Contains(withoutNum));
                    if (rightID == null)
                        return false;
                    enemy.unitID = rightID;
                }
            foreach (var ally in world.mainCharacter.characterGroup)
            {
                if (!_units.Contains(ally.unitID))
                {
                    var withoutNum = ally.unitID.Substring(ally.unitID.IndexOf('_') + 1);
                    string rightID = _units.FirstOrDefault(stringToCheck => stringToCheck.Contains(withoutNum));
                    if (rightID == null)
                        return false;
                    ally.unitID = rightID;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
        
        return true;
    }
    
    public void AddButton(string worldName)
    {
        GameObject go = Instantiate(worldButtonPrefab);
        go.GetComponent<WorldButton>().SetButton(worldName);
        go.transform.SetParent(worldButtonParent.transform);
        go.transform.localScale = new Vector3(1, 1, 1);
    }
    
    
}

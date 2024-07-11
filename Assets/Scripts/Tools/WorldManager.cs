using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;

/// <summary>
/// Class that is responsible for storing and managing Game Worlds.
/// </summary>
public class WorldManager : MonoBehaviour
{

    [HideInInspector]
    public WorldButton currentWorld = null;
    
    [SerializeField]
    private Button newGameButton;
    
    private TextMeshProUGUI _newGameText;

    /// <summary>
    /// Contains all created Game Worlds.
    /// </summary>
    [HideInInspector]
    public List<string> Worlds = new List<string>();
    
    /// <summary>
    /// Contains all created Game Worlds.
    /// </summary>
    public GameObject worldButtonPrefab;
    public GameObject worldButtonParent;

    public bool inputDisabled = false;
    public static WorldManager worldManager  { get; private set; }
    
    public List<Button> buttonsToDisable = new List<Button>();

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

        _newGameText = newGameButton.GetComponentInChildren<TextMeshProUGUI>();

        var info = new DirectoryInfo(Application.streamingAssetsPath + @"/Worlds");
        var directories = info.GetDirectories();
        foreach (var directory in directories)
        {
            var directoryName = directory.Name;
            //if (directoryName.Contains(".meta"))
                //continue;
            var worldName = directoryName;
            //worldManager.Worlds.Add(worldName);
            worldManager.AddButton(worldName);
        }

        CheckNewGame();
    }

    public void DeleteInterface(WorldButton button)
    {
        GetComponent<MainMenu>().CallDeleteInterface(button);
    }

    public void SetCurrentWorld(WorldButton wb)
    {
        
        currentWorld = wb;
        CheckNewGame();
    }
    
    public void CheckNewGame()
    {
        if (worldManager.currentWorld == null)
        {
            newGameButton.gameObject.GetComponent<Button>().interactable = false;
            _newGameText.color = new Color(1, 1, 1, 0.4f);
        }
        else
        {
            newGameButton.gameObject.GetComponent<Button>().interactable = true;
            _newGameText.color = new Color(1, 1, 1, 1f);
        }

    }
    
    public void AddButton(string worldName)
    {
        GameObject go = Instantiate(worldButtonPrefab, worldButtonParent.transform);
        go.GetComponentInChildren<WorldButton>().SetButton(worldName);
        go.transform.localScale = new Vector3(1, 1, 1);
        foreach (var button in go.GetComponentsInChildren<Button>())
        {
            buttonsToDisable.Add(button);
        }

    }
    
    public void DisableInput()
    {
        foreach (Button button in buttonsToDisable)
        {
            if (button != null && !button.Equals(null) && !button.IsDestroyed())
                button.interactable = false; 
        }
    }
    
    public void EnableInput()
    {
        foreach (Button button in buttonsToDisable)
        {
            if (button != null && !button.Equals(null) && !button.IsDestroyed())
                button.interactable = true; 
        }
    }
}

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
    /// <summary>
    /// Currently chosen Game World.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Currently chosen Game World.")]
    public WorldButton currentWorld = null;
    
    [SerializeField]
    private Button newGameButton;
    
    private TextMeshProUGUI _newGameText;

    /// <summary>
    /// Contains all created Game Worlds.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Contains all created Game Worlds.")]
    public List<string> Worlds = new List<string>();
    
    /// <summary>
    /// Prefab for UI Button object representing the gameWorld.
    /// </summary>
    [UnityEngine.Tooltip("Prefab for UI Button object representing the gameWorld.")]
    public GameObject worldButtonPrefab;
    /// <summary>
    /// ScrollView content object.
    /// </summary>
    [UnityEngine.Tooltip("ScrollView content object.")]
    public GameObject worldButtonParent;

    /// <summary>
    /// Is input currently disabled.
    /// </summary>
    [UnityEngine.Tooltip("Is input currently disabled.")]
    public bool inputDisabled = false;
    /// <summary>
    /// Static object representing the class.
    /// </summary>
    public static WorldManager worldManager  { get; private set; }
    /// <summary>
    /// Buttons that should be disabled when the world is being created.
    /// </summary>
    [UnityEngine.Tooltip("Buttons that should be disabled when the world is being created.")]
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

    /// <summary>
    /// Method that is called from the World Button. Makes the call to the method that opens deleting interface in the Main Menu.
    /// </summary>
    /// <param name="button">World Button that is to be deleted in the deleting interface.</param>
    public void DeleteInterface(WorldButton button)
    {
        GetComponent<MainMenu>().CallDeleteInterface(button);
    }

    /// <summary>
    /// Method that is called from the World Button. Sets the Game World that will be played.
    /// </summary>
    /// <param name="button">World Button that is to be deleted in the deleting interface.</param>
    public void SetCurrentWorld(WorldButton button)
    {
        
        currentWorld = button;
        CheckNewGame();
    }
    
    /// <summary>
    /// Method that checks if the New Game Button should be set to be interactable.
    /// </summary>
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
    
    /// <summary>
    /// Method that adds buttons for the corresponding Game World for existing ones when the game is launched and when the new Game World is created.
    /// </summary>
    /// <param name="worldName"> String that is shown on the Game World Button.</param>
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
    
    /// <summary>
    /// Disable all the buttons in the Main Menu.
    /// </summary>
    public void DisableInput()
    {
        foreach (Button button in buttonsToDisable)
        {
            if (button != null && !button.Equals(null) && !button.IsDestroyed())
                button.interactable = false; 
        }
    }
    
    /// <summary>
    /// Enable all the buttons in the Main Menu.
    /// </summary>
    public void EnableInput()
    {
        foreach (Button button in buttonsToDisable)
        {
            if (button != null && !button.Equals(null) && !button.IsDestroyed())
                button.interactable = true; 
        }
    }
}

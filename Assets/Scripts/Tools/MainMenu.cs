using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main menu logic and controls.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Button that starts the game when the world is chosen.
    /// </summary>
    [SerializeField]
    private Button newGameButton;

    /// <summary>
    /// Button that closes the application.
    /// </summary>
    [SerializeField] 
    private Button quiteButton;

    private WorldButton _currentlyDeletedWorld = null;

    /// <summary>
    /// UI window where player is asked whether they want to delete the game world.
    /// </summary>
    [SerializeField] private GameObject deletingInterface;
    /// <summary>
    /// Name of the being deleted world.
    /// </summary>
    [SerializeField] private TextMeshProUGUI deletingWorldName;
    /// <summary>
    /// Sound that is being played when the button in menu is clicked.
    /// </summary>
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    void Start()
    {
        deletingInterface.SetActive(false);
    }
    /// <summary>
    /// Called from the Main Menu. Button is active when the game world is chosen.
    /// </summary>
    public void OnPlayGameClicked()
    {
        SceneController.LoadScene(1, 1, 1, 0.8f);
        WorldManager.worldManager.DisableInput();
        WorldManager.worldManager.inputDisabled = true;
        AudioManager.instance.Play(ButtonClick);
    }

    /// <summary>
    /// Called from deleting interface. Deletes game world files and button representing it.
    /// </summary>
    public void OnClickedDelete()
    {
        string worldFolderPath = Application.streamingAssetsPath + @"/Worlds/" +
                                 _currentlyDeletedWorld.folderName;
        File.Delete(worldFolderPath + ".meta");

        if (Directory.Exists(worldFolderPath))
        {
            Directory.Delete(worldFolderPath, true); 
        }
        WorldManager.worldManager.Worlds.Remove(_currentlyDeletedWorld.worldName.text);
        Destroy(_currentlyDeletedWorld.transform.parent.gameObject);
        WorldManager.worldManager.currentWorld = null;
        WorldManager.worldManager.CheckNewGame();
        
        deletingInterface.SetActive(false);
        WorldManager.worldManager.EnableInput();
        _currentlyDeletedWorld = null;
        WorldManager.worldManager.inputDisabled = false;
        AudioManager.instance.Play(ButtonClick);
    }
    
    /// <summary>
    /// Called from deleting interface. Cancels deletion process, exiting from deleting interface.
    /// </summary>
    public void OnClickedCancel()
    {
        deletingInterface.SetActive(false);
        WorldManager.worldManager.EnableInput();
        _currentlyDeletedWorld = null;
        WorldManager.worldManager.inputDisabled = false;
        AudioManager.instance.Play(ButtonClick);
    }
    
    /// <summary>
    /// Opens deleting interface.
    /// </summary>
    /// <param name="button">World Button that is to be deleted in the deleting interface.</param>
    public void CallDeleteInterface(WorldButton button)
    {
        if (WorldManager.worldManager.inputDisabled)
            return;
        WorldManager.worldManager.DisableInput();
        _currentlyDeletedWorld = button;
        deletingWorldName.text = button.worldName.text + " ?";
        WorldManager.worldManager.inputDisabled = true;
        deletingInterface.SetActive(true);
    }

    /// <summary>
    /// Called from the Main Menu. Closes the game.
    /// </summary>
    public void OnQuitGameClicked()
    {
        AudioManager.instance.Play(ButtonClick);
        //ButtonClickedSound();
        Application.Quit();
    }
    

}

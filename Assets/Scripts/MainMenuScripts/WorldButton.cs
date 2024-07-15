using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
/// <summary>
/// Script for the UI element representing the Game World Button.
/// </summary>
public class WorldButton : MonoBehaviour
{
    /// <summary>
    /// Name of the Game World represented by this button.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Name of the Game World represented by this button.")]
    public TextMeshProUGUI worldName;

    /// <summary>
    /// Name of the folder containing JSON representation of the Game World.
    /// </summary>
    [HideInInspector] public string folderName;
    /// <summary>
    /// Image for the button that is calling the deleting interface.
    /// </summary>
    [HideInInspector] public Image image;
    
    /// <summary>
    /// Sound that is being played when the button is clicked.
    /// </summary>
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    [UnityEngine.Tooltip("Sound that is being played when the button is clicked.")]
    public Sound ButtonClick;
    
    /// <summary>
    /// Called when this Game World Button is clicked. Sets the new current game world and changes the color of the button.
    /// </summary>
    public void OnClicked()
    {
        if (WorldManager.worldManager.inputDisabled)
            return;
        if (WorldManager.worldManager.currentWorld != null)
        {
            WorldManager.worldManager.currentWorld.image.color = Color.white;
        }
        WorldManager.worldManager.SetCurrentWorld(this);
        image.color = new Color32(245, 124, 124, 255);
        AudioManager.instance.Play(ButtonClick);
    }
    
    /// <summary>
    /// Called when the button is created.
    /// </summary>
    /// <param name="fn">Folder name to be parsed.</param>
    public void SetButton(string fn)
    {
        folderName = fn;
        image = GetComponent<Image>();
        worldName = GetComponentInChildren<TextMeshProUGUI>();
        worldName.text = folderName.Replace("_", " ");
    }
    
    /// <summary>
    /// Called when the delete part of the Game World Button is clicked.
    /// </summary>
    public void DeleteWorld()
    {
        AudioManager.instance.Play(ButtonClick);
        WorldManager.worldManager.DeleteInterface(this);
    }
}
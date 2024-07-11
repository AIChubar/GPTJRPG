using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WorldButton : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI worldName;

    [HideInInspector] public string folderName;
    [HideInInspector] public Image image;
    
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
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
    public void SetButton(string fn)
    {
        folderName = fn;
        image = GetComponent<Image>();
        worldName = GetComponentInChildren<TextMeshProUGUI>();
        worldName.text = System.Text.RegularExpressions.Regex.Replace(folderName, @"(\B[A-Z])", " $1");
    }
    
    public void DeleteWorld()
    {
        AudioManager.instance.Play(ButtonClick);
        WorldManager.worldManager.DeleteInterface(this);
    }
}
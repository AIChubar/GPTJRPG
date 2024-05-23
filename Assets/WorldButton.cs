using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public void OnClicked()
    {
        if (WorldManager.worldManager.currentWorld != null)
        {
            WorldManager.worldManager.currentWorld.image.color = Color.white;
        }
        WorldManager.worldManager.currentWorld = this;
        image.color = new Color32(245, 124, 124, 255);
    }
    public void SetButton(string fn)
    {
        folderName = fn;
        image = GetComponent<Image>();
        worldName = GetComponentInChildren<TextMeshProUGUI>();
        worldName.text = System.Text.RegularExpressions.Regex.Replace(folderName, @"(\B[A-Z])", " $1");
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WorldManager : MonoBehaviour
{
    // Start is called before the first frame update

    private List<Button> _worldButtons;
    public GameObject worldButtonPrefab;
    public GameObject worldButtonParent;
    void Start()
    {
        var info = new DirectoryInfo(Application.dataPath + @"/Resources/Worlds" + "\"");
        var fileInfo = info.GetFiles();
        foreach (var file  in fileInfo)
        {
            _worlds
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

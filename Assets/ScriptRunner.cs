using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScriptRunner : MonoBehaviour
{
    public GameObject creating;

    private List<string> _units = new List<string>(); 
    
    void Start()
    {
        using StreamReader sr = new StreamReader(Application.dataPath + "/Resources/API/enemies.txt");
        while (sr.ReadLine() is { } line)
        {
            _units.Add(line);
        }
        
        var info = new DirectoryInfo(Application.dataPath + @"/Resources/Worlds");
        var fileInfo = info.GetFiles();
        foreach (var file  in fileInfo)
        {
            if (file.Name.Contains(".meta"))
                continue;
            var worldName = file.Name.Substring(0, file.Name.Length - 5);
            var jsonWorld =
                JsonUtility.FromJson<JSONReader.GameWorld>(Resources.Load<TextAsset>("Worlds/" + worldName).text);
            if (!CheckUnits(jsonWorld))
                continue;
            WorldManager.worldManager.Worlds.Add(worldName, jsonWorld);
            WorldManager.worldManager.AddButton(worldName);
            
        }
    }

    public void GenerateNewWorld()
    {
        StartCoroutine(RunScriptAndManageObject());
    }

    private bool CheckUnits(JSONReader.GameWorld world)
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

        foreach (var ally in world.mainCharacter.characterGroup.units)
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
        return true;
    }

    IEnumerator RunScriptAndManageObject()
    {
        // Activate the object 1 second before launching the PowerShell script
        creating.SetActive(true);
        yield return new WaitForSeconds(1f);

        // Run the script
        var scriptArguments = "-ExecutionPolicy Bypass -File \"" + Application.dataPath + @"/Resources/API/PSscript.ps1" + "\"";
        var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments);
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = true;

        using (var process = new Process())
        {
            JSONReader.GameWorld world;
            do
            {
                process.StartInfo = processStartInfo;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                world = JsonUtility.FromJson<JSONReader.GameWorld>(output);
                
            } while (!CheckUnits(world));
            WorldManager.worldManager.Worlds.Add(world.narrativeData.worldName, world);
            AddNewButton(world.narrativeData.worldName);
        }

        yield return new WaitForSeconds(0.33f);
        creating.SetActive(false);

        // Add new button after deactivating the object
    }

    private void AddNewButton(string name)
    {
        WorldManager.worldManager.AddButton(name);
    }

}

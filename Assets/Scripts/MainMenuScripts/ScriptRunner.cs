using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

/// <summary>
/// Script that creates a new Game World.
/// </summary>
public class ScriptRunner : MonoBehaviour
{
    public GameObject creating;
    /// <summary>
    /// This method calls a coroutine that runs a Shell Script. Which runs several Python Scripts to create the Game World.
    /// </summary>
    public void GenerateNewWorld()
    {
        StartCoroutine(RunScriptAndManageObject());
    }
    IEnumerator RunScriptAndManageObject()
    {
        creating.SetActive(true);
        WorldManager.worldManager.inputDisabled = true;

        WorldManager.worldManager.DisableInput();
        var scriptArguments = "-ExecutionPolicy Bypass -File \"" + Application.streamingAssetsPath + @"/API/create_world.ps1" + "\"";
        var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments);
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = true;

        using (var process = new Process())
        {
            process.StartInfo = processStartInfo;
            string output = ""; 
            string error = "";

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    output += e.Data + "\n";
                }
            };
            
            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    error += e.Data + "\n";
                }
            };
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            while (!process.HasExited)
            {
                yield return null; 
            }

            WorldManager.worldManager.inputDisabled = false;
            creating.SetActive(false);
            WorldManager.worldManager.EnableInput();

            if (error != "")
            {
                Debug.LogError(error);

                yield break;
            }
            
            string currentWorldPath = Path.Combine(Application.streamingAssetsPath, "API", "current_world.json");
            var currentWorld = JsonConvert.DeserializeObject<JSONReader.CurrentWorld>(File.ReadAllText(currentWorldPath));
            AddNewButton(currentWorld.worldName);
        }

    }

    private void AddNewButton(string name)
    {
        WorldManager.worldManager.AddButton(name);
    }

}

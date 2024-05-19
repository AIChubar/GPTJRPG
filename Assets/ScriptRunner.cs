using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ScriptRunner : MonoBehaviour
{
    public GameObject creating;
    public void GenerateNewWorld()
    {
        StartCoroutine(RunScriptAndManageObject());
    }
    
    public void DeleteCurrentWorld()
    {
        if (WorldManager.worldManager.currentWorld == null)
            return;
        File.Delete(Application.streamingAssetsPath + @"/Worlds/" + WorldManager.worldManager.currentWorld.worldName.text + ".json");
        File.Delete(Application.streamingAssetsPath + @"/Worlds/" + WorldManager.worldManager.currentWorld.worldName.text + ".json.meta");
        WorldManager.worldManager.Worlds.Remove(WorldManager.worldManager.currentWorld.worldName.text);
        Destroy(WorldManager.worldManager.currentWorld.gameObject);
    }

    

    IEnumerator RunScriptAndManageObject()
    {
        creating.SetActive(true);

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
                yield return null; // Or any other delay mechanism
            }
            creating.SetActive(false);

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

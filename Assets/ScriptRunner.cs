using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ScriptRunner : MonoBehaviour
{
    public GameObject creating;

    
    void Start()
    {
        
    }

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
        yield return new WaitForSeconds(1.2f); // because animation is not working while world is creating

        var scriptArguments = "-ExecutionPolicy Bypass -File \"" + Application.streamingAssetsPath + @"/API/PSscript.ps1" + "\"";
        var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments);
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
        processStartInfo.UseShellExecute = false;
        processStartInfo.CreateNoWindow = true;

        using (var process = new Process())
        {
            int attempts = 0;
            JSONReader.GameWorld world = null;
            do
            {
                if (attempts >= 3)
                {
                    Debug.LogError("Too many attempts!");
                    yield break;
                }
                attempts++;
                process.StartInfo = processStartInfo;
                process.Start();
            
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                if (error != "")
                    Debug.LogError(error);
                try
                {
                    world = JsonUtility.FromJson<JSONReader.GameWorld>(output);
                }
                catch (Exception e)
                {
                    Debug.LogError("Not JSON parsable output: \n" + output);
                }
            } while (world == null || !WorldManager.worldManager.CheckUnits(world));
            WorldManager.worldManager.Worlds.Add(world.narrativeData.worldName, world);
            AddNewButton(world.narrativeData.worldName);
        }

        yield return new WaitForSeconds(0.33f);
        creating.SetActive(false);

    }

    private void AddNewButton(string name)
    {
        WorldManager.worldManager.AddButton(name);
    }

}

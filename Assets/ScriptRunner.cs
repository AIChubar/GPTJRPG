using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ScriptRunner : MonoBehaviour
{

    void Awake()
    {
        
    }

    public void GenerateNewWorld()
    {
        var scriptArguments = "-ExecutionPolicy Bypass -File \"" + Application.dataPath + @"/Resources/API/PSscript.ps1" + "\"";
        var processStartInfo = new ProcessStartInfo("powershell.exe", scriptArguments);
        processStartInfo.RedirectStandardOutput = true;
        processStartInfo.RedirectStandardError = true;
    
        using var process = new Process();
        process.StartInfo = processStartInfo;
        process.StartInfo.UseShellExecute = false;
        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        Console.WriteLine(output);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class GeneratePaletteNamesFile : MonoBehaviour
{
    /*[MenuItem("Tools/Generate palette list file")]
    public static void GenerateFile()
    {
        var info = new DirectoryInfo(Application.dataPath + @"/TilePalettes/Palettes");
        var fileInfo = info.GetFiles();
        string filePath = Path.Combine(Application.streamingAssetsPath+"/API/", "palettes.txt");
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var file in fileInfo)
            {
                var fileName = file.Name;
                if (fileName.Contains(".meta"))
                    continue;
                writer.WriteLine(fileName.Substring(0, file.Name.Length - 7));
            }
        }
    }*/
}
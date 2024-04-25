using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

    [CreateAssetMenu(fileName = "PalettesData", menuName = "ScriptableObjects/PalettesData", order = 2)]
    public class PalettesData : ScriptableObject
    {
        public Dictionary<string, GameObject> TerrainPalettes;
        
    }
    
    
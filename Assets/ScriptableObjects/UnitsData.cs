using System;
using System.Collections.Generic;
using UnityEngine;

    [CreateAssetMenu(fileName = "UnitsData", menuName = "ScriptableObjects/UnitsData", order = 1)]
    public class UnitsData : ScriptableObject
    {
        public List<GroupData> unitGroups;
    }
    
    [Serializable]
    public class GroupData
    {
        public string name;
        
        public List<UnitData> units;
    }

    [Serializable]
    public class UnitData
    {
        public string name;

        public int maxHP;
        
        public int currentHP;

        public int damage;

        public bool friendly;
    }
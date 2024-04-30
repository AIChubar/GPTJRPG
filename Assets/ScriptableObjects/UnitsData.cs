using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

    [CreateAssetMenu(fileName = "UnitsData", menuName = "ScriptableObjects/UnitsData", order = 1)]
    public class UnitsData : ScriptableObject
    {
        public List<GroupData> enemyGroups;

        public GroupData allyGroup;
    }
    
    [Serializable]
    public class GroupData
    {
        public string name;

        public string battleStartMonologue;
        
        public string winMonologue;

        public string lostMonologue;
        
        public List<UnitData> units;
    }

    [Serializable]
    public class UnitData
    {
        public string id;
        
        public string name;

        public int maxHP;
        
        public int currentHP;

        public int damage;

        public bool friendly;
    }
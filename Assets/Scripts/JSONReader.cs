using System.Collections.Generic;
using UnityEngine;
public class JSONReader : MonoBehaviour
{
    private GameWorld gameWorld;
    public UnitsData unitsData;
    
    [System.Serializable]
    public class GameWorld
    {
        public NarrativeData narrativeData;
        public MainCharacter mainCharacter;
        public Level[] levels;
    }

    [System.Serializable]
    public class NarrativeData
    {
        public string worldName;
        public string story;
        public string greetingGameMessage;
        public string finalGameMessage;
    }

    [System.Serializable]
    public class MainCharacter
    {
        public string name;
        public string race;
        public string characterClass;
        public string occupation;
        public string backStory;
        public UnitJSON[] characterGroup;
    }

    [System.Serializable]
    public class Level
    {
        public string levelName;
        public string tilePalette;
        public UnitGroup[] enemyGroups;
        public QuestJSON[] questList;

    }
    [System.Serializable]
    public class QuestJSON
    {
        public string questName;
        public string questType;
        public string questObjective;
        public int objectiveNum;
    }

    [System.Serializable]
    public class UnitGroup
    {
        public string groupName;
        public string battleStartMonologue;
        public string winMonologue;
        public string lostMonologue;
        public UnitJSON[] units;
    }
    
    [System.Serializable]
    public class UnitJSON
    {
        public string unitID;
        public string unitName;
        public int health;
        public int damage;
    }
    void Awake()
    {
        gameWorld = WorldManager.worldManager.Worlds[WorldManager.worldManager.currentWorld.worldName.text];
    
        unitsData.enemyGroups = new List<GroupData>();
        foreach (var t in gameWorld.levels[0].enemyGroups)
        {
            GroupData newGroup = new GroupData();
            newGroup.units = new List<UnitData>();
            newGroup.name = t.groupName;
            newGroup.battleStartMonologue = t.battleStartMonologue;
            newGroup.winMonologue = t.winMonologue;
            newGroup.lostMonologue = t.lostMonologue;

            foreach (var unit in t.units)
            {
                UnitData newUnit = new UnitData();
                newUnit.name = unit.unitName;
                newUnit.id = unit.unitID;
                newUnit.damage = unit.damage;
                newUnit.friendly = false;
                newUnit.currentHP = newUnit.maxHP = unit.health;
                newGroup.units.Add(newUnit);
            }
            unitsData.enemyGroups.Add(newGroup);
        }
        
        GroupData allyGroup = new GroupData();
        allyGroup.units = new List<UnitData>();
        foreach (var unit in gameWorld.mainCharacter.characterGroup)
        {
            
            UnitData newUnit = new UnitData();
            newUnit.name = unit.unitName;
            newUnit.id = unit.unitID;
            newUnit.damage = unit.damage;
            newUnit.currentHP = newUnit.maxHP = unit.health;
            newUnit.friendly = true;
            allyGroup.units.Add(newUnit);
        }

        unitsData.allyGroup = allyGroup;
        
        GameManager.gameManager.world = gameWorld;
    }
}

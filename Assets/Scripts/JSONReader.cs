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
        public UnitGroup characterGroup;
        public string characterClass;
        public string occupation;
        public string backStory;
    }

    [System.Serializable]
    public class Level
    {
        public string name;
        public string friendlyCharacter;
        public string monologue;
        public UnitGroup[] enemyGroups;
        public string[] uniqueSprites;
        public Tile walkableTile;
        public Tile nonWalkableTile;
    }

    [System.Serializable]
    public class Tile
    {
        public string colour;
        public string texture;
    }
    [System.Serializable]
    public class UnitGroup
    {
        public string groupName;
        public UnitJSON[] units;
    }
    
    [System.Serializable]
    public class UnitJSON
    {
        public string unitName;
        public string unitID;
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
            foreach (var unit in t.units)
            {
                UnitData newUnit = new UnitData();
                newUnit.name = unit.unitName;
                newUnit.id = unit.unitID;
                newUnit.damage = unit.damage;
                newUnit.currentHP = newUnit.maxHP = unit.health;
                newGroup.units.Add(newUnit);
            }
            unitsData.enemyGroups.Add(newGroup);
        }
        
        GroupData allyGroup = new GroupData();
        allyGroup.units = new List<UnitData>();
        allyGroup.name = gameWorld.mainCharacter.name;
        foreach (var unit in gameWorld.mainCharacter.characterGroup.units)
        {
            
            UnitData newUnit = new UnitData();
            newUnit.name = unit.unitName;
            newUnit.id = unit.unitID;
            newUnit.damage = unit.damage;
            newUnit.currentHP = newUnit.maxHP = unit.health;
            allyGroup.units.Add(newUnit);
        }

        unitsData.allyGroup = allyGroup;
    }
}

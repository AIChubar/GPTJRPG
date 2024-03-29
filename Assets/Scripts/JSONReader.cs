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
    }

    [System.Serializable]
    public class Level
    {
        public string name;
        public string friendlyCharacter;
        public string monologue;
        public EnemyGroup[] enemyGroups;
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
    public class EnemyGroup
    {
        public string groupName;
        public UnitJSON[] units;
    }
    [System.Serializable]
    public class UnitJSON
    {
        public string enemyName;
        public string enemyID;
        public int health;
        public int damage;
    }
    void Awake()
    {
        gameWorld = WorldManager.worldManager.Worlds[WorldManager.worldManager.currentWorld.worldName.text];
    
        unitsData.unitGroups = new List<GroupData>();
        foreach (var t in gameWorld.levels[0].enemyGroups)
        {
            GroupData newGroup = new GroupData();
            newGroup.units = new List<UnitData>();
            newGroup.name = t.groupName;
            foreach (var k in t.units)
            {
                UnitData newUnit = new UnitData();
                newUnit.name = k.enemyName;
                newUnit.id = k.enemyID;
                newUnit.damage = k.damage;
                newUnit.currentHP = newUnit.maxHP = k.health;
                newGroup.units.Add(newUnit);
            }
            unitsData.unitGroups.Add(newGroup);
        }
    }
}

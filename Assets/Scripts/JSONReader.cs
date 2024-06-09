using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using Newtonsoft.Json;
public class JSONReader : MonoBehaviour
{
    private GameWorld gameWorld = new GameWorld();

    private UnitsStats _unitsStats;
    
    [System.Serializable]
    public class GameWorld
    {
        public NarrativeData narrativeData;
        public MainCharacter mainCharacter;
        public Level[] levels;
        public UnitsData unitsData;
        public QuestLevel[] questData;
        [SerializeReference]
        public Dictionary<string, DialogueInfo> dialogues = new Dictionary<string, DialogueInfo>();
    }

    [System.Serializable]
    public class NarrativeData
    {
        public string worldName;
        public string story;
        public string greetingGameMessage;
        public string mainVillain;
        public string defeatEndingMessage;
        public string gameOverMessage;
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
    public class Levels
    {
        public Level[] levels;

    }
    [System.Serializable]
    public class QuestLevels
    {
        public QuestLevel[] questLevels;

    }
    [System.Serializable]
    public class Level
    {
        public string levelName;
        public string walkableTerrain;
        public string obstacleTerrain;
        public string levelDescription;

    }
    [System.Serializable]
    public class QuestLevel
    {
        public string levelName;
        public QuestJSON[] questList;

    }
    [System.Serializable]
    public class QuestJSON
    {
        public string questName;
        public string questDescription;
        public string questType;
        public string questObjective;
        public string questReward;
    }
    [System.Serializable]
    public class UnitsData
    {
        public UnitGroup friendlyGroup;
        public LevelUnits[] levelsUnits;
        public UnitGroup mainVillain;
    }
    
    [System.Serializable]
    public class LevelUnits
    {
        public string levelName;
        public UnitGroup[] enemyGroups;
    }
    [System.Serializable]
    public class UnitGroup
    {
        public string groupName;
        public List<UnitJSON> units;
    }

    [System.Serializable]
    public class UnitJSON
    {
        public string firstAttribute;
        public string secondAttribute;
        public string thirdAttribute;
        public string characteristicName;
        public string artisticName;
        public string powerLevel;
        public string unitType;
        
        public int maxHP;
        public int currentHP;
        public int damage;
        public int armour;
        public bool friendly;
    }

    [System.Serializable]
    public class CurrentWorld
    {
        public string worldName;
        public UnitJSON[] enemiesMain;
    }
    
    [System.Serializable]
    public class DialogueInfo
    {
        public EnemySpeakerInfo enemySpeakerInfo;
        [SerializeReference]
        public DialogueTree dialogueTree;
    }
    [System.Serializable]
    public class EnemySpeakerInfo
    {
        public UnitJSON unit;
        public Metrics personaMetrics;
    }
    
    [System.Serializable]
    public class Metrics
    {
        public int aggressiveness;
        public int friendliness;
        public int intellect;
        public int strategicThinking;
        public int indifference;
        public int flexibility;
        public int communicationSkills;
        public int morality;
        public int chaoticity;
    }
    [System.Serializable]
    public class DialogueTree
    {
        public string enemyPhrase;
        [SerializeReference]
        public PlayerCharacterAnswer[] playerCharacterAnswers;
    }
    
    [System.Serializable]
    public class PlayerCharacterAnswer
    {
        public string playerOption;
        public string enemyAnswer;
        [SerializeReference]
        public PlayerCharacterAnswer[] playerCharacterAnswers; 
        [CanBeNull] public string outcome; 
    }
    
    [System.Serializable]
    public class UnitsStats
    {
        public Dictionary<string, UnitTypeCoefficient> unitType;
        public Dictionary<string, PowerLevelAttribute> powerLevelAttributes;
    }
    
    [System.Serializable]
    public class UnitTypeCoefficient
    {
        public float healthCoefficient;
        public float damageCoefficient;
        public float armourCoefficient;
    }

    [System.Serializable]
    public class PowerLevelAttribute
    {
        public int health;
        public int damage;
        public int armour;
    }
    

    void Awake()
    {
        var gameWorldName = WorldManager.worldManager.currentWorld.folderName;
        string folderPath = Path.Combine(Application.streamingAssetsPath, "Worlds", gameWorldName);
        string unitsStatsPath = Path.Combine(Application.streamingAssetsPath, "API", "units_stats.json");
        
        string narrativePath = Path.Combine(folderPath, "Narrative.json");
        string mainCharacterPath = Path.Combine(folderPath, "MainCharacter.json");
        string levelsPath = Path.Combine(folderPath, "Levels.json");
        string unitDataPath = Path.Combine(folderPath, "UnitData.json");
        string questDataPath = Path.Combine(folderPath, "QuestData.json");
        
        string narrativeJsonText = File.ReadAllText(narrativePath);
        string mainCharacterJsonText = File.ReadAllText(mainCharacterPath);
        string levelsJsonText = File.ReadAllText(levelsPath);
        string unitDataJsonText = File.ReadAllText(unitDataPath);
        string questDataJsonText = File.ReadAllText(questDataPath);
        
        string unitsStatsText = File.ReadAllText(unitsStatsPath);
        
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore  
        };
        _unitsStats = JsonConvert.DeserializeObject<UnitsStats>(unitsStatsText, settings);
        
        gameWorld.narrativeData =  JsonConvert.DeserializeObject<NarrativeData>(narrativeJsonText,settings);
        gameWorld.mainCharacter = JsonConvert.DeserializeObject<MainCharacter>(mainCharacterJsonText,settings);
        var levelsJson = JsonConvert.DeserializeObject<Levels>(levelsJsonText,settings);
        gameWorld.unitsData = JsonConvert.DeserializeObject<UnitsData>(unitDataJsonText,settings);
        var questDataJson = JsonConvert.DeserializeObject<QuestLevels>(questDataJsonText,settings);
        gameWorld.levels = levelsJson.levels;
        gameWorld.questData = questDataJson.questLevels;
        
        string dialoguesFolderPath = Path.Combine(Application.streamingAssetsPath, "Worlds", gameWorldName, "Dialogues");

        // Get all file paths in the directory
        string[] fileNames = Directory.GetFiles(dialoguesFolderPath);
        
        foreach (string file in fileNames)
        {
            if (file.Contains(".meta"))
                continue;
            string fileContents = File.ReadAllText(file);
            var dialogueJson = JsonConvert.DeserializeObject<DialogueInfo>(fileContents,settings);
            
            gameWorld.dialogues.Add(dialogueJson.enemySpeakerInfo.unit.artisticName, dialogueJson);
        }
        AssignStats();

        GameManager.gameManager.world = gameWorld;
    }

    private void AssignStats()
    {
        AssignStatsForGroup(gameWorld.unitsData.friendlyGroup.units, true);
        AssignStatsForGroup(gameWorld.unitsData.mainVillain.units, false);
        foreach (var level in gameWorld.unitsData.levelsUnits)
        {
            foreach (var group in level.enemyGroups)
            {
                AssignStatsForGroup(group.units, false);
            }
        }
    }

    private void AssignStatsForGroup(List<UnitJSON> units, bool isFriendly)
    {
        foreach (var unit in units)
        {
            var unitPowerLevel = _unitsStats.powerLevelAttributes[unit.powerLevel];
            var unitType = _unitsStats.unitType[unit.unitType];
            unit.friendly = isFriendly;
            unit.damage = (int)(unitPowerLevel.damage * unitType.damageCoefficient);
            unit.maxHP = unit.currentHP = (int)(unitPowerLevel.health * unitType.healthCoefficient);
            unit.armour = (int)(unitPowerLevel.armour * unitType.armourCoefficient);
        }
    }

}

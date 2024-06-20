using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButton : MonoBehaviour
{    
    
    [HideInInspector] public Image image;

    [HideInInspector]
    private TextMeshProUGUI _questNameTMP;

    [HideInInspector] public JSONReader.QuestJSON quest;
    
    public bool completed = false;
    
    public bool failed = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GameEvents_OnUnitKilled(Unit unit)
    {
        if (quest.questObjective == unit.unitData.artisticName)
        {
            if (quest.questType == "unite")
            {
                failed = true;
            }
            else
            {
                completed = true;
                if (quest.questReward == "amulet of alliance")
                {
                    GameManager.gameManager.hero.amuletOfAlliance++;
                }
                else if (quest.questReward == "amulet of healing")
                {
                    GameManager.gameManager.hero.amuletOfHealing++;
                }
                else
                {
                    GameObject.FindGameObjectWithTag("LevelDoor").GetComponent<DoorScript>().OpenDoor();
                }
            }
            
            GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
            GameEvents.gameEvents.OnUnitUnited -= GameEvents_OnUnitUnited;
        }
    }

    private void GameEvents_OnUnitUnited(JSONReader.UnitJSON unitData)
    {
        if (quest.questObjective == unitData.artisticName)
        {
            if (quest.questType == "kill")
            {
                failed = true;
            }
            else
            {
                completed = true;
                if (quest.questReward == "amulet of alliance")
                {
                    GameManager.gameManager.hero.amuletOfAlliance++;
                }
                else if (quest.questReward == "amulet of healing")
                {
                    GameManager.gameManager.hero.amuletOfHealing++;
                }
                else
                {
                    GameObject.FindGameObjectWithTag("LevelDoor").GetComponent<DoorScript>().OpenDoor();
                }
            }
            GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
            GameEvents.gameEvents.OnUnitUnited -= GameEvents_OnUnitUnited;
        }
    }
    
    public void SetQuest(JSONReader.QuestJSON _quest)
    {
        quest = _quest;
        image = GetComponent<Image>();

        _questNameTMP = GetComponentInChildren<TextMeshProUGUI>();
        if (quest.questName.Length >= 18)
            _questNameTMP.text = quest.questName.Substring(0, 18) + "...";
        else
            _questNameTMP.text = quest.questName;
        GameEvents.gameEvents.OnUnitKilled += GameEvents_OnUnitKilled;
        GameEvents.gameEvents.OnUnitUnited += GameEvents_OnUnitUnited;
    }
}

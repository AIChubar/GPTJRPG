using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Script for the UI element representing the Quest Button. Exists inside the <see cref="QuestMenu"/> UI.
/// </summary>
public class QuestButton : MonoBehaviour
{    

    private TextMeshProUGUI _questNameTMP;

    /// <summary>
    /// Quest that is represented by this Quest Button.
    /// </summary>
    [HideInInspector] public JSONReader.QuestJSON quest;

    [HideInInspector]
    public bool completed = false;
    [HideInInspector]
    public bool failed = false;

    private void GameEvents_OnUnitKilled(Unit unit)
    {
        if (quest.questObjective == unit.unitData.artisticName || quest.questObjective == unit.unitData.characteristicName)
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
        if (quest.questObjective == unitData.artisticName || quest.questObjective == unitData.characteristicName)
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
    
    /// <summary>
    /// Sets up the quest.
    /// </summary>
    /// <param name="q">Quest that is going to be represented by this Quest Button.</param>
    public void SetQuest(JSONReader.QuestJSON q)
    {
        quest = q;
        _questNameTMP = GetComponentInChildren<TextMeshProUGUI>();
        if (quest.questName.Length >= 18)
            _questNameTMP.text = quest.questName.Substring(0, 18) + "...";
        else
            _questNameTMP.text = quest.questName;
        GameEvents.gameEvents.OnUnitKilled += GameEvents_OnUnitKilled;
        GameEvents.gameEvents.OnUnitUnited += GameEvents_OnUnitUnited;
    }
}

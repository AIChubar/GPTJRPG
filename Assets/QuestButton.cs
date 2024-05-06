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

    public int objectiveCount;

    [HideInInspector] public JSONReader.QuestJSON quest;
    
    [HideInInspector]
    public bool completed = false;
    
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
        if (quest.questObjective == unit.unitData.name)
            objectiveCount++;
        if (objectiveCount >= quest.objectiveNum)
        {
            GameEvents.gameEvents.OnUnitKilled -= GameEvents_OnUnitKilled;
            completed = true;
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
        if (quest.questType == "kill")
        {
            GameEvents.gameEvents.OnUnitKilled += GameEvents_OnUnitKilled;
        }
    }
}

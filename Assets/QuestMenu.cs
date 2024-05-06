using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestMenu : MonoBehaviour
{
    [HideInInspector]
    public QuestButton currentQuest;
    
    public GameObject questButtonPrefab;
    public GameObject questButtonParent;

    [SerializeField] private TextMeshProUGUI chosenQuestNameTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestObjectiveTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestProgressTMP;

    

    // Start is called before the first frame update
    void Start()
    {
        var questList = GameManager.gameManager.world.levels[GameManager.gameManager.levelIndex].questList;

        foreach (var quest in questList)
        {
            AddButton(quest);
        }
    }
    
    public void OnClicked(GameObject button)
    {
        if (currentQuest != null)
        {
            currentQuest.gameObject.GetComponent<Image>().color = Color.white;
        }
        currentQuest = button.GetComponent<QuestButton>();
        chosenQuestNameTMP.text = currentQuest.quest.questName;
        chosenQuestObjectiveTMP.text = currentQuest.quest.questType + ": " + currentQuest.quest.questObjective;
        chosenQuestProgressTMP.text = currentQuest.objectiveCount + "/" + currentQuest.quest.objectiveNum;
        button.GetComponent<Image>().color = !currentQuest.completed ? new Color32(245, 124, 124, 255) : new Color32(124, 245, 124, 255);
    }

    // Update is called once per frame
    public void AddButton(JSONReader.QuestJSON quest)
    {
        GameObject go = Instantiate(questButtonPrefab, questButtonParent.transform);
        var button = go.GetComponent<QuestButton>();
        go.GetComponent<Button>().onClick.AddListener(delegate { OnClicked(go);});
        button.SetQuest(quest);
        go.transform.localScale = new Vector3(1, 1, 1);
    }
}

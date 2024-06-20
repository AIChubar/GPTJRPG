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
    [SerializeField] private GameObject questDetails;
    
    [SerializeField] private TextMeshProUGUI chosenQuestNameTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestDescriptionTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestObjectiveTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestRewardTMP;

    public GameObject questPiecePrefab;
    public GameObject questHUD;
    public GameObject questPieceParent;
    // Start is called before the first frame update
    public void SetQuests()
    {
        foreach (var quest in questButtonParent.GetComponentsInChildren<QuestButton>())
        {
            if (!quest.completed && !quest.failed)
            {
                quest.failed = true;
            }
        }
        var questList = GameManager.gameManager.world.questData[GameManager.gameManager.levelIndex].questList;
        questDetails.SetActive(false);
        chosenQuestNameTMP.text = "";
        chosenQuestDescriptionTMP.text = "";
        chosenQuestObjectiveTMP.text = "";
        chosenQuestRewardTMP.text = "";
        foreach (var quest in questList)
        {
            AddButton(quest);
        }

        UpdateQuestHUD();
    }

    public void UpdateQuestHUD()
    {
        int children = questPieceParent.transform.childCount;
        for (int i = 0; i < children; ++i)
            Destroy(questPieceParent.transform.GetChild(i).gameObject);

        foreach (var quest in questButtonParent.GetComponentsInChildren<QuestButton>())
        {
            if (!quest.completed && !quest.failed)
            {
                GameObject go = Instantiate(questPiecePrefab, questPieceParent.transform);
                go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.quest.questName;
                go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = char.ToUpper(quest.quest.questType[0]) + quest.quest.questType.Substring(1) + ": " + quest.quest.questObjective;
            }
        }
    }
    public void OnClicked(GameObject button)
    {
        if (currentQuest is not null && !currentQuest.Equals(null))
        {
            if (currentQuest.completed)
            {
                currentQuest.GetComponent<Image>().color = new Color32(124, 245, 124, 255);
            }
            else if (currentQuest.failed)
            {
                currentQuest.GetComponent<Image>().color = new Color32(245, 124, 124, 255);

            }
            else
            {
                currentQuest.GetComponent<Image>().color = new Color32(255,255,255, 255);

            }
        }
        questDetails.SetActive(true);
        currentQuest = button.GetComponent<QuestButton>();
        chosenQuestNameTMP.text = currentQuest.quest.questName;
        chosenQuestDescriptionTMP.text = currentQuest.quest.questDescription;
        chosenQuestObjectiveTMP.text = char.ToUpper(currentQuest.quest.questType[0]) + currentQuest.quest.questType.Substring(1) + ": " + currentQuest.quest.questObjective;
        chosenQuestRewardTMP.text = currentQuest.quest.questReward;
        if (currentQuest.completed)
        {
            currentQuest.GetComponent<Image>().color = new Color32(124, 245, 124, 150);
        }
        else if (currentQuest.failed)
        {
            currentQuest.GetComponent<Image>().color = new Color32(245, 124, 124, 150);

        }
        else
        {
            currentQuest.GetComponent<Image>().color = new Color32(255,255,255, 150);

        }
    }

    public void UpdateButtons()
    {
        foreach (var quest in questButtonParent.GetComponentsInChildren<QuestButton>())
        {
            if (quest.completed)
            {
                quest.GetComponent<Image>().color = new Color32(124, 245, 124, 255);
            }
            else if (quest.failed)
            {
                quest.transform.SetAsLastSibling();
                quest.GetComponent<Image>().color = new Color32(245, 124, 124, 255);

            }
            else
            {
                quest.transform.SetAsFirstSibling();
                quest.GetComponent<Image>().color = new Color32(255,255,255, 255);

            }
        }
        
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

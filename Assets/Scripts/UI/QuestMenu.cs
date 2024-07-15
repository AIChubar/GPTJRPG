using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Script that controls the Quest Menu Window UI element.
/// </summary>
public class QuestMenu : MonoBehaviour
{
    /// <summary>
    /// Quest Button selected inside the Quest Menu.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Quest Button selected inside the Quest Menu.")]
    public QuestButton currentQuest;
    
    /// <summary>
    /// Prefab for the <see cref="QuestButton"/>.
    /// </summary>
    [UnityEngine.Tooltip("Prefab for the QuestButton.")]
    public GameObject questButtonPrefab;
    /// <summary>
    /// ScrollView content object for Quest Buttons.
    /// </summary>
    [UnityEngine.Tooltip("ScrollView content object for Quest Buttons.")]
    public GameObject questButtonParent;
    
    /// <summary>
    /// ScrollView object that contains details about the chosen quest.
    /// </summary>
    [SerializeField] private GameObject questDetails;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI chosenQuestNameTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestDescriptionTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestObjectiveTMP;
    [SerializeField] private TextMeshProUGUI chosenQuestRewardTMP;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    
    /// <summary>
    /// Prefab for the <see cref="questDetails"/> information chunk.
    /// </summary>
    [UnityEngine.Tooltip("Prefab for the questDetails information chunk.")]
    public GameObject questPiecePrefab;
    
    /// <summary>
    /// ScrollView content object for <see cref="questDetails"/> information chunks.
    /// </summary>
    [UnityEngine.Tooltip("ScrollView content object for questDetails information chunks.")]
    public GameObject questPieceParent;
    
    /// <summary>
    /// Quest HUD that is showing the minimized information about currently available quests.
    /// </summary>
    [UnityEngine.Tooltip("Quest HUD that is showing the minimized information about currently available quests.")]
    public GameObject questHUD;
    /// <summary>
    /// Sets up the all <see cref="QuestButton"/>.
    /// </summary>
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

    /// <summary>
    /// Updates the <see cref="questHUD"/> content.
    /// </summary>
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
    
    /// <summary>
    /// Method that is delegated to the <see cref="QuestButton"/>. Chooses the new <see cref="currentQuest"/>.
    /// </summary>
    public void OnClicked(GameObject button)
    {
        AudioManager.instance.Play(ButtonClick);
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
    /// <summary>
    /// Updates the position and the colour of <see cref="QuestButton"/>s depending on their status.
    /// </summary>
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
    
    /// <summary>
    /// Adds new Quest Button to the list.
    /// </summary>
    /// <param name="quest">Quest data for the added Quest Button.</param>
    public void AddButton(JSONReader.QuestJSON quest)
    {
        GameObject go = Instantiate(questButtonPrefab, questButtonParent.transform);
        var button = go.GetComponent<QuestButton>();
        go.GetComponent<Button>().onClick.AddListener(delegate { OnClicked(go);});
        button.SetQuest(quest);
        go.transform.localScale = new Vector3(1, 1, 1);
    }
}

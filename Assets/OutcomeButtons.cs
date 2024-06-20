using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeButtons : MonoBehaviour
{
    [SerializeField] private Button fight;

    [SerializeField] private Button ignore;
    
    [SerializeField] private Button recruit;

    [SerializeField] private Button amulet;

    private DialogueHUD _dialogueHUD;
    // Start is called before the first frame update
    public void SetOutcomeButtons(DialogueHUD.Outcome outcome, DialogueHUD dh)
    {
        _dialogueHUD = dh;
        fight.gameObject.SetActive(true);
        ignore.gameObject.SetActive(outcome is not DialogueHUD.Outcome.FIGHT);
        recruit.gameObject.SetActive(outcome is DialogueHUD.Outcome.RECRUIT);
        amulet.gameObject.SetActive(outcome is not DialogueHUD.Outcome.RECRUIT && !GameManager.gameManager.gameData.isBossFight);
        var amulets = GameManager.gameManager.hero.amuletOfAlliance;    
        amulet.GetComponentInChildren<TextMeshProUGUI>().text = "Recruit with an amulet: " + amulets;
        amulet.interactable = amulets > 0;
    }

    public void OnClickedFight()
    {
        GameManager.gameManager.pauseMenu.BeginBattle();
    }

    public void OnClickedIgnore()
    {
        GameManager.gameManager.pauseMenu.ResumeGame();
    }

    public void OnClickedRecruit()
    {
        GameManager.gameManager.pauseMenu.ShowRecruitSystemMenu(_dialogueHUD.group);
        foreach (var unitData in _dialogueHUD.group.units)
        {
            GameEvents.gameEvents.UnitUnited(unitData);
        }
    }
    
    public void OnClickedAmuletRecruit()
    {
        if (GameManager.gameManager.hero.amuletOfAlliance < 1)
            return;
        GameManager.gameManager.hero.amuletOfAlliance--;
        GameManager.gameManager.pauseMenu.ShowRecruitSystemMenu(_dialogueHUD.group);
        foreach (var unitData in _dialogueHUD.group.units)
        {
            GameEvents.gameEvents.UnitUnited(unitData);
        }
    }
}

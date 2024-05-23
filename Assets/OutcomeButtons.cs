using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutcomeButtons : MonoBehaviour
{
    [SerializeField] private Button fight;

    [SerializeField] private Button ignore;
    
    [SerializeField] private Button recruit;

    private DialogueHUD _dialogueHUD;
    // Start is called before the first frame update
    public void SetOutcomeButtons(DialogueHUD.Outcome outcome, DialogueHUD dh)
    {
        _dialogueHUD = dh;
        fight.gameObject.SetActive(true);
        ignore.gameObject.SetActive(outcome is not DialogueHUD.Outcome.FIGHT);
        recruit.gameObject.SetActive(outcome is DialogueHUD.Outcome.RECRUIT);
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
        OnClickedIgnore();
    }
}

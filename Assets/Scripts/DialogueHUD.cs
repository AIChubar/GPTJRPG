using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//add hp change notification
public class DialogueHUD : MonoBehaviour
{
    public enum Outcome { RECRUIT, IGNORE, FIGHT }
    [SerializeField] 
    private TextMeshProUGUI artisticNameText;
    
    [SerializeField] 
    private TextMeshProUGUI characteristicNameText;
    
    [SerializeField] 
    private JSONReader.DialogueInfo dialogueInfo;
    
    [SerializeField] 
    private Image unitImage;

    private Outcome _outcome;
    
    [SerializeField] 
    private Button continueButton;

    private List<Button> _currentDialogueButtons = new List<Button>();

    [SerializeField]
    private OutcomeButtons outcomeButtons;
    
    [HideInInspector] public JSONReader.PlayerCharacterAnswer chosenOption = null;
    
    public GameObject playerDialoguePrefab;
    public GameObject enemyDialoguePrefab;
    public GameObject dialogueButtonPrefab;
    public RectTransform content;
    private void Start()
    {
    }

    public void SetDialogueHUD(JSONReader.DialogueInfo di)
    {
        dialogueInfo = di;

        //_continueText = continueButton.GetComponentInChildren<TextMeshProUGUI>();

        if (unitImage is not null)
        {
            
            unitImage.sprite = GameManager.gameManager.GetSprite(dialogueInfo.enemySpeakerInfo.unit);
            unitImage.color = Color.white;
        }
        artisticNameText.text = dialogueInfo.enemySpeakerInfo.unit.artisticName;
        characteristicNameText.text = dialogueInfo.enemySpeakerInfo.unit.characteristicName;
        
        StartCoroutine(ProcessDialogue());
        
        

    }

    private IEnumerator AddPhrase(string phrase, bool isPlayer, bool animate = true)
    {
        GameObject prefab = isPlayer ? playerDialoguePrefab : enemyDialoguePrefab;
        GameObject dialogueItem = Instantiate(prefab, content);
        TextMeshProUGUI textComponent = dialogueItem.GetComponentInChildren<TextMeshProUGUI>();
        textComponent.text = "";
        if (animate)
        {
            foreach (var t in phrase)
            {
                textComponent.text += t;
                yield return new WaitForSecondsRealtime(0.025f);
            }
        }
        else
        {
            textComponent.text = phrase;
            yield return null;
        }

        /*RectTransform rt = dialogueItem.GetComponent<RectTransform>();
        rt.pivot = piece.isPlayer ? new Vector2(1, 1) : new Vector2(0, 1);
        rt.anchorMin = piece.isPlayer ? new Vector2(1, 1) : new Vector2(0, 1);
        rt.anchorMax = piece.isPlayer ? new Vector2(1, 1) : new Vector2(0, 1);*/
    }

    private IEnumerator AddOptionButton(JSONReader.PlayerCharacterAnswer option)
    {
        GameObject go = Instantiate(dialogueButtonPrefab, content);
        Button b = go.GetComponent<Button>();
        b.interactable = false;
        yield return go.GetComponent<DialogueButton>().SetButton(option, this);
        
        _currentDialogueButtons.Add(b);
    }

    private IEnumerator ProcessDialogue()
    {
        outcomeButtons.gameObject.SetActive(false);

        JSONReader.PlayerCharacterAnswer[] currentOptions = dialogueInfo.dialogueTree.playerCharacterAnswers;
        chosenOption = null;
        yield return AddPhrase(dialogueInfo.dialogueTree.enemyPhrase, false);
        while (chosenOption == null || chosenOption.outcome == null )
        {
            chosenOption = null;
            foreach (var option in currentOptions)
            {
                yield return AddOptionButton(option);
            }

            foreach (var button in _currentDialogueButtons)
            {
                button.interactable = true;
            }
            while (chosenOption == null)
            {
                yield return null;
            }

            currentOptions = chosenOption.playerCharacterAnswers;

            foreach (var button in _currentDialogueButtons)
            {
                Destroy(button.gameObject);
            }
            yield return AddPhrase(chosenOption.playerOption, true, false);
            yield return AddPhrase(chosenOption.enemyAnswer, false);
            _currentDialogueButtons.Clear();
        }

        if (chosenOption.outcome == "ally")
        {
            _outcome = Outcome.RECRUIT;
        }
        else if (chosenOption.outcome == "ignore")
        {
            _outcome = Outcome.IGNORE;
        }
        else
        {
            _outcome = Outcome.FIGHT;
        }
        yield return new WaitForSecondsRealtime(1f);
        
        outcomeButtons.gameObject.SetActive(true);
        outcomeButtons.SetOutcomeButtons(_outcome, this);
    }

    /*IEnumerator TextAppear()
    {
        
    }*/
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Script that controls the Dialogue Window UI element.
/// </summary>
public class DialogueHUD : MonoBehaviour
{
    /// <summary>
    /// Enum, which members represent what is the outcome of the dialogue.
    /// </summary>
    public enum Outcome { RECRUIT, IGNORE, FIGHT }
    /// <summary>
    /// Text UI object for <see cref="JSONReader.UnitJSON.artisticName"/> of the current <see cref="WorldEnemy"/>.
    /// </summary>
    [SerializeField] 
    private TextMeshProUGUI artisticNameText;
    /// <summary>
    /// Text UI object for <see cref="JSONReader.UnitJSON.characteristicName"/> of the current <see cref="WorldEnemy"/>.
    /// </summary>
    [SerializeField] 
    private TextMeshProUGUI characteristicNameText;
    
    private JSONReader.DialogueInfo dialogueInfo;
    
    /// <summary>
    /// <see cref="JSONReader.UnitGroup"/> of the current <see cref="WorldEnemy"/>.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("JSONReader.UnitGroup of the current WorldEnemy")]
    public JSONReader.UnitGroup group;
    
    /// <summary>
    /// Image UI object representing current <see cref="WorldEnemy"/>.
    /// </summary>
    [SerializeField] 
    private Image unitImage;

    private Outcome _outcome = Outcome.RECRUIT;

    private List<Button> _currentDialogueButtons = new List<Button>();

    [SerializeField]
    private OutcomeButtons outcomeButtons;
    
    /// <summary>
    /// Last chosen <see cref="JSONReader.PlayerCharacterAnswer"/>.
    /// </summary>
    [HideInInspector] public JSONReader.PlayerCharacterAnswer chosenOption = null;
    
    /// <summary>
    /// Prefab of the simple UI object containing text of the player dialogue phrase.
    /// </summary>
    [UnityEngine.Tooltip("Prefab of the simple UI object containing text of the player dialogue phrase.")]
    public GameObject playerDialoguePrefab;
    /// <summary>
    /// Prefab of the simple UI object containing text of the enemy dialogue phrase.
    /// </summary>
    [UnityEngine.Tooltip("Prefab of the simple UI object containing text of the enemy dialogue phrase.")]
    public GameObject enemyDialoguePrefab;
    /// <summary>
    /// Prefab of the Button UI representing <see cref="JSONReader.PlayerCharacterAnswer.playerCharacterAnswers"/>.
    /// </summary>
    [UnityEngine.Tooltip("Prefab of the Button UI representing JSONReader.PlayerCharacterAnswer.playerCharacterAnswers.")]
    public GameObject dialogueButtonPrefab;
    /// <summary>
    /// ScrollView content object.
    /// </summary>
    [UnityEngine.Tooltip("ScrollView content object.")]
    public RectTransform content;

    /// <summary>
    /// Set up the Dialogue UI window.
    /// </summary>
    /// <param name="di">Current <see cref="WorldEnemy"/> dialogue data.</param>
    /// <param name="g">Current <see cref="WorldEnemy"/> unit group.</param>
    public void SetDialogueHUD(JSONReader.DialogueInfo di, JSONReader.UnitGroup g)
    {
        group = g;
        foreach(Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        
        dialogueInfo = di;

        if (dialogueInfo.enemySpeakerInfo == null)
        {
            StartCoroutine(ProcessDialogue(false));
            return;
        }
        
        unitImage.sprite = GameManager.gameManager.GetSpriteUnit(dialogueInfo.enemySpeakerInfo.unit);
        unitImage.color = Color.white;
            
        artisticNameText.text = dialogueInfo.enemySpeakerInfo.unit.artisticName;
        characteristicNameText.text = dialogueInfo.enemySpeakerInfo.unit.characteristicName;
        
        StartCoroutine(ProcessDialogue(true));
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
    }

    private IEnumerator AddOptionButton(JSONReader.PlayerCharacterAnswer option)
    {
        GameObject go = Instantiate(dialogueButtonPrefab, content);
        Button b = go.GetComponent<Button>();
        b.interactable = false;
        yield return go.GetComponent<DialogueButton>().SetButton(option, this);
        
        _currentDialogueButtons.Add(b);
    }

    private IEnumerator ProcessDialogue(bool valid)
    {
        if (valid)
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

            if (GameManager.gameManager.gameData.isBossFight)
                _outcome = Outcome.FIGHT;
            yield return new WaitForSecondsRealtime(1f);

        }

        yield return null;
        outcomeButtons.gameObject.SetActive(true);
        outcomeButtons.SetOutcomeButtons(_outcome, this);
    }

}

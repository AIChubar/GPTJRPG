using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script for the Button UI representing <see cref="JSONReader.PlayerCharacterAnswer.playerCharacterAnswers"/>.
/// </summary>
public class DialogueButton : MonoBehaviour
{
    [HideInInspector]
    public TextMeshProUGUI optionText;

    private DialogueHUD _dialogueHUD;

    private JSONReader.PlayerCharacterAnswer _optionBranch;
    [Dropdown("AudioManager.Instance.Sounds", "Name")]
    public Sound ButtonClick;
    public void OnClicked()
    {
        _dialogueHUD.chosenOption = _optionBranch;
        AudioManager.instance.Play(ButtonClick);
    }
    /// <summary>
    /// Prefab of the Button UI representing <see cref="JSONReader.PlayerCharacterAnswer.playerCharacterAnswers"/>.
    /// </summary>
    /// <param name="option">Dialogue option that is represented by this button.</param>
    /// <param name="dh">Dialogue UI Object.</param>
    public IEnumerator SetButton(JSONReader.PlayerCharacterAnswer option, DialogueHUD dh)
    {
        _optionBranch = option;
        _dialogueHUD = dh;
        optionText = GetComponentInChildren<TextMeshProUGUI>();
        optionText.text = "";
        foreach (var t in _optionBranch.playerOption)
        {
            optionText.text += t;
            yield return new WaitForSecondsRealtime(0.015f);
        }
    }
}

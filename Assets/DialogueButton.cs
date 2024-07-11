using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//add hp change notification
public class DialogueHUD : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI groupNameText;
    
    [SerializeField] 
    private TextMeshProUGUI monologueText;

    private string _monologueString;
    
    [SerializeField] 
    private Image unitImage;

    [SerializeField] 
    private Button continueButton;
    
    private int _maxHp;

    private UnitData _currentUnitData;
    
    private TextMeshProUGUI _continueText;

    private void Start()
    {
    }

    public void SetHUD(GroupData groupData, string monologue)
    {
        _continueText = continueButton.GetComponentInChildren<TextMeshProUGUI>();

        if (unitImage is not null)
        {
            var unitNameForSpriteAtlas = groupData.units[0].id.Substring(groupData.units[0].id.IndexOf('_') + 1);
            unitImage.sprite = GameManager.gameManager.atlas.GetSprite(unitNameForSpriteAtlas);
            unitImage.color = Color.white;
        }
        groupNameText.text = groupData.name;
        monologueText.text = "";
        _monologueString = monologue;
        StartCoroutine(TextAppear());
    }

    IEnumerator TextAppear()
    {
        continueButton.interactable = false;
        _continueText.color = new Color(1, 1, 1, 0.4f);
        foreach (var t in _monologueString)
        {
            monologueText.text += t;
            yield return new WaitForSecondsRealtime(0.033f);
        }
        continueButton.interactable = true;
        _continueText.color = new Color(1, 1, 1, 1f);
    }
}

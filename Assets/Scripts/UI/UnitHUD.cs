using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Script that controls Unit UI element.
/// </summary>
public class UnitHUD : MonoBehaviour
{
    [Header("Text UI objects for information about the Unit")]
    public TextMeshProUGUI artisticNameText;
    public TextMeshProUGUI characteristicNameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;
    /// <summary>
    /// Image UI object.
    /// </summary>
    [UnityEngine.Tooltip("Image UI object.")]
    public Image unitImage;
    
    public JSONReader.UnitJSON _currentUnitData = null;
    /// <summary>
    /// Sets up the Unit UI element 
    /// </summary>
    /// <param name="ud">Data about the unit.</param>
    /// <param name="sprite">In case it is given, assigned for <see cref="unitImage"/>.</param>
    public void SetHUD(JSONReader.UnitJSON ud, Sprite sprite = null)
    {
        if (unitImage is not null && ud is not null)
        {
            if (sprite is null)
            {
                unitImage.sprite = GameManager.gameManager.GetSpriteUnit(ud);
            }
            else
            {
                unitImage.sprite = sprite;
            }
            unitImage.color = Color.white;
        }
        else if (ud is null)
        {
            unitImage.color = Color.clear;
        }
        
        _currentUnitData = ud;
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (_currentUnitData is null || _currentUnitData.artisticName == "")
        {
            artisticNameText.text = "";
            characteristicNameText.text = "";
            hpText.text = "";
            damageText.text = "";
            return; 
        }
        artisticNameText.text = _currentUnitData.artisticName;
        characteristicNameText.text = _currentUnitData.characteristicName + ", " + _currentUnitData.powerLevel + " " + _currentUnitData.unitClass;
        int maxHp = _currentUnitData.maxHP;
        damageText.text = "Damage: " + _currentUnitData.damage + "   " + "Armour: " + _currentUnitData.armour;
        hpText.text ="HP: " + _currentUnitData.currentHP + " / " + maxHp;
    }

 
    
}

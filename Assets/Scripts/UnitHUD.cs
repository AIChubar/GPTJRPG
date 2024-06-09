using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UnitHUD : MonoBehaviour
{
    public TextMeshProUGUI artisticNameText;
    public TextMeshProUGUI characteristicNameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;

    public Image unitImage;
    
    private int _maxHp; 

    public JSONReader.UnitJSON _currentUnitData = null;

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

    public void UpdateHUD()
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
        characteristicNameText.text = _currentUnitData.characteristicName + ", " + _currentUnitData.powerLevel + " " + _currentUnitData.unitType;
        _maxHp = _currentUnitData.maxHP;
        damageText.text = "Damage: " + _currentUnitData.damage + "   " + "Armour: " + _currentUnitData.armour;
        hpText.text ="HP: " + _currentUnitData.currentHP + " / " + _maxHp;
    }

    public void SetHP(int hp)
    {
        hpText.text = hp + " / " + _maxHp;
    }
    
}

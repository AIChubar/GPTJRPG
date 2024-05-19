using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//add hp change notification
public class UnitHUD : MonoBehaviour
{
    public TextMeshProUGUI artisticNameText;
    public TextMeshProUGUI characteristicNameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;

    public Image unitImage;
    
    private int _maxHp;

    private JSONReader.UnitJSON _currentUnitData;

    public void SetHUD(JSONReader.UnitJSON ud)
    {
        if (unitImage is not null)
        {
            unitImage.sprite = GameManager.gameManager.GetSprite(ud);
            unitImage.color = Color.white;
        }
        _currentUnitData = ud;
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        if (_currentUnitData is null)
        {
            artisticNameText.text = "";
            characteristicNameText.text = "";
            hpText.text = "";
            damageText.text = "";
            return; 
        }
        artisticNameText.text = _currentUnitData.artisticName;
        characteristicNameText.text = _currentUnitData.characteristicName;
        _maxHp = _currentUnitData.maxHP;
        damageText.text = "Damage: " + _currentUnitData.damage;
        hpText.text ="HP: " + _currentUnitData.currentHP + " / " + _maxHp;
    }

    public void SetHP(int hp)
    {
        hpText.text = hp + " / " + _maxHp;
    }
    
}

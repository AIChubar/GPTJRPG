using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//add hp change notification
public class UnitHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI damageText;

    public Image unitImage;
    
    private int _maxHp;

    private UnitData _currentUnitData;

    public void SetHUD(UnitData unitData)
    {
        if (unitImage != null)
        {
            unitImage.sprite = GameManager.gameManager.atlas.GetSprite(unitData.name);
        }
        _currentUnitData = unitData;
        UpdateHUD();
    }

    public void UpdateHUD()
    {
        if (_currentUnitData is null)
        {
            nameText.text = "";
            hpText.text = "";
            damageText.text = "";
            return; 
        }
        nameText.text = _currentUnitData.name;
        _maxHp = _currentUnitData.maxHP;
        damageText.text = "Damage: " + _currentUnitData.damage;
        hpText.text ="HP: " + _currentUnitData.currentHP + " / " + _maxHp;
    }

    public void SetHP(int hp)
    {
        hpText.text = hp + " / " + _maxHp;
    }
    
}

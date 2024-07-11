using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InfoHUD : MonoBehaviour
{
    public TextMeshProUGUI firstHeader;
    public TextMeshProUGUI secondHeader;
    public TextMeshProUGUI thirdHeader;
    public TextMeshProUGUI firstRow;
    public TextMeshProUGUI secondRow;
    public TextMeshProUGUI thirdRow;

    public Image image;
    

    public void SetHUD(JSONReader.UnitJSON ud, Sprite sprite = null)
    {
        if (image is not null && ud is not null)
        {
            if (sprite is null)
            {
                image.sprite = GameManager.gameManager.GetSpriteUnit(ud);
            }
            else
            {
                image.sprite = sprite;
            }
            image.color = Color.white;
        }
        else if (ud is null )
        {
            if (image is not null)
                image.color = Color.clear;
        }
        
        if (ud is null || ud.artisticName == "")
        {
            firstHeader.text = "";
            firstRow.text = "";
            secondRow.text = "";
            thirdRow.text = "";
            return; 
        }

        secondHeader.text = "";
        thirdHeader.text = "";
        firstHeader.text = ud.artisticName;
        firstRow.text = ud.characteristicName + ", " + ud.powerLevel + " " + ud.unitClass;
        int maxHp = ud.maxHP;
        thirdRow.text = "Damage: " + ud.damage + "   " + "Armour: " + ud.armour;
        secondRow.text ="HP: " + ud.currentHP + " / " + maxHp;
    }

    public void ResetHUD()
    {
        firstHeader.text = "";
        secondHeader.text = "";
        thirdHeader.text = "";
        firstRow.text = "";
        secondRow.text = "";
        thirdRow.text = "";
        image.color = Color.clear;
    }

    public void SetHUD(ObjectForInfoHUD oi, Sprite sprite = null)
    {
        if (image is not null && oi is not null)
        {
            if (sprite is null)
            {
                image.sprite = oi.image.sprite;
            }
            else
            {
                image.sprite = sprite;
            }
            image.color = Color.white;
        }
        else if (oi is null )
        {
            if (image is not null)
                image.color = Color.clear;
            return;
        }
        firstHeader.text = oi.firstRow;
        secondHeader.text = oi.secondRow;
        thirdHeader.text = oi.thirdRow;
        firstRow.text = "";
        secondRow.text = "";
        thirdRow.text = "";
    }
    
}

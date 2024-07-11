using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GroupInfoHUD : SwappingInterface
{
    [SerializeField] private UnitHUD[] unitHUDs;

    [SerializeField] private Button healButton;
    
     private TextMeshProUGUI _healButtonTMP;
     
     [SerializeField] private TextMeshProUGUI healingCounter;
     [SerializeField] private TextMeshProUGUI allianceCounter;
    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        _healButtonTMP = healButton.GetComponentInChildren<TextMeshProUGUI>();
        UpdateHUD();
    }

    // Update is called once per frame
    public void UpdateHUD()
    {
        if (GameManager.gameManager.hero is null || items.Count == 0) 
            return;
        var _hero = GameManager.gameManager.hero; 
        for (int i = 0; i < _hero.allyGroup.units.Count; i++)
        {
            unitHUDs[i].SetHUD(_hero.allyGroup.units[i]);
        }

        healingCounter.text = _hero.amuletOfHealing.ToString();
        allianceCounter.text = _hero.amuletOfAlliance.ToString();
        if (_hero.amuletOfHealing > 0)
        {
            healButton.interactable = true;
            _healButtonTMP.color = new Color(1, 1, 1, 1f);

        }
        else
        {
            healButton.interactable = false;
            _healButtonTMP.color = new Color(1, 1, 1, 0.4f);
        }
        for (int i = _hero.allyGroup.units.Count; i < unitHUDs.Length; i++)
        {
            unitHUDs[i].SetHUD(null);
            items[i].active = false;
        }
    }

    protected override void Swap(int currentIndex, int targetIndex)
    {
        if (!items[targetIndex].active || !items[currentIndex].active)
            return;
        var currentUnit = unitHUDs[currentIndex]._currentUnitData;
        var targetUnit = unitHUDs[targetIndex]._currentUnitData;
        unitHUDs[currentIndex].SetHUD(targetUnit);
        unitHUDs[targetIndex].SetHUD(currentUnit);

        for (int i = 0; i < GameManager.gameManager.hero.allyGroup.units.Count; i++)
        {
            GameManager.gameManager.hero.allyGroup.units[i] = unitHUDs[i]._currentUnitData;
        }

    }

    public void OnHealButtonClicked()
    {
        if (GameManager.gameManager.hero.amuletOfHealing > 0)
        {
            foreach (var unit in GameManager.gameManager.hero.allyGroup.units)
            {
                unit.currentHP = unit.maxHP;
            } 
            GameManager.gameManager.hero.amuletOfHealing--;

        }
            
        UpdateHUD();
    }
}

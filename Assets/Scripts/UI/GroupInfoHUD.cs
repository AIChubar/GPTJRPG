using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script that controls the Group Info Window UI element.
/// </summary>
public class GroupInfoHUD : SwappingInterface
{
    /// <summary>
    /// UI objects representing <see cref="JSONReader.UnitJSON"/> of the player's group.
    /// </summary>
    [SerializeField] private UnitHUD[] unitHUDs;

    /// <summary>
    /// UI Button that calls <see cref="OnHealButtonClicked"/> method.
    /// </summary>
    [SerializeField] private Button healButton;
    
     private TextMeshProUGUI _healButtonTMP;
     
     /// <summary>
     /// Text UI object for the number of available healing amulets.
     /// </summary>
     [SerializeField] private TextMeshProUGUI healingCounter;
     /// <summary>
     /// Text UI object for the number of available alliance amulets.
     /// </summary>
     [SerializeField] private TextMeshProUGUI allianceCounter;
    // Start is called before the first frame update
    private new void Start()
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

    /// <summary>
    /// Overriden method of the <see cref="SwappingInterface"/>. Swaps dragged object with one under the mouth cursor.
    /// </summary>
    /// <param name="currentIndex">Index of the dragged object.</param>
    /// <param name="targetIndex">Index of the object under the mouth cursor.</param>
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
    /// <summary>
    /// Heals all <see cref="JSONReader.UnitJSON"/> of the player's group.
    /// </summary>
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

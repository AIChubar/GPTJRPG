using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupInfoHUD : SwappingInterface
{
    [SerializeField] private UnitHUD[] unitHUDs;
    
    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
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
    
    
}

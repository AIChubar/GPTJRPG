using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecruitSystem : SwappingInterface
{
    
    [SerializeField] private UnitHUD[] unitHUDs;
    
    private Hero _hero;

    private JSONReader.UnitGroup _enemyGroup;
    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        _hero = GameManager.gameManager.hero;
        UpdateHUD();
    }

    public void SetEnemyGroup(JSONReader.UnitGroup group)
    {
        _enemyGroup = group;
        if (_hero is not null)
            UpdateHUD();
    }

    public void UpdateHUD()
    {
        if (_hero is null)
            return;
        for (int i = 0; i < _hero.allyGroup.units.Count; i++)
        {
            unitHUDs[i].SetHUD(_hero.allyGroup.units[i]);
            items[i].active = true;
        }

        for (int i = _hero.allyGroup.units.Count; i < 3; i++)
        {
            unitHUDs[i].SetHUD(null);
            items[i].active = false;
        }
        
        for (int i = 0; i < _enemyGroup.units.Count ; i++)
        {
            unitHUDs[i + 3].SetHUD(_enemyGroup.units[i]);
            items[i+3].active = true;
        }
        
        for (int i = _enemyGroup.units.Count + 3; i < 6; i++)
        {
            unitHUDs[i].SetHUD(null);
            items[i].active = false;
        }
    }

    protected override void Swap(int currentIndex, int targetIndex)
    {
        if (!items[targetIndex].active && (currentIndex <= 2 || targetIndex > 2))
            return;
        bool recruitingNew = !items[targetIndex].active && targetIndex <= 2;
        
        var currentUnit = unitHUDs[currentIndex]._currentUnitData;
        var targetUnit = unitHUDs[targetIndex]._currentUnitData;
        unitHUDs[currentIndex].SetHUD(targetUnit);
        unitHUDs[targetIndex].SetHUD(currentUnit);

        
        if (recruitingNew)
        {
            items[targetIndex].active = true;
            items[currentIndex].active = false;
            _hero.allyGroup.units.Add(unitHUDs[targetIndex]._currentUnitData);
        }
        
        for (int i = 0; i < _hero.allyGroup.units.Count; i++)
        {
            _hero.allyGroup.units[i] = unitHUDs[i]._currentUnitData;
        }

    }

}

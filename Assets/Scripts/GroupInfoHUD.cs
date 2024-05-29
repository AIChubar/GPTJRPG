using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupInfoHUD : SwappingInterface
{
    [SerializeField] private UnitHUD[] groupHUDs;
    
    private Hero _hero;
    
    // Start is called before the first frame update
    private void Start()
    {
        base.Start();
        _hero = GameManager.gameManager.hero;
        UpdateHUD()
    }

    // Update is called once per frame
    private void UpdateHUD()
    {
        for (int i = 0; i < _hero.allyGroup.units.Count; i++)
        {
            groupHUDs[i].SetHUD(_hero.allyGroup.units[i]);
        }

        for (int i = _hero.allyGroup.units.Count; i < groupHUDs.Length - _hero.allyGroup.units.Count; i++)
        {
            
        }
    }

    protected override void Swap(int currentIndex, int targetIndex)
    {
        var currentUnit = groupHUDs[currentIndex]._currentUnitData;
        var targetUnit = groupHUDs[targetIndex]._currentUnitData;
        groupHUDs[currentIndex].SetHUD(targetUnit);
        groupHUDs[targetIndex].SetHUD(currentUnit);

        for (int i = 0; i < _hero.allyGroup.units.Count; i++)
        {
            _hero.allyGroup.units[i] = groupHUDs[i]._currentUnitData;
        }

    }
    
    
}

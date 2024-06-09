using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//make killed game event
public class GameEvents : MonoBehaviour
{
    public static GameEvents gameEvents;
    // Start is called before the first frame update
    
    private void Awake()
    {
        gameEvents = this;
    }
    void Start()
    {
        
    }

    public event Action<Unit> OnUnitKilled;
    
    public event Action<JSONReader.UnitJSON> OnUnitUnited;
    public event Action<Unit> OnUnitHPChanged;
    public void UnitKilled(Unit unit)
    {
        if (OnUnitKilled != null)
        {
            OnUnitKilled(unit);
        }
    }
    
    public void UnitUnited(JSONReader.UnitJSON unit)
    {
        if (OnUnitUnited != null)
        {
            OnUnitUnited(unit);
        }
    }

    public void UnitHPChanged(Unit unit)
    {
        if (OnUnitHPChanged != null)
        {
            OnUnitHPChanged(unit);
        }
    }
}
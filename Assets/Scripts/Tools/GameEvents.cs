using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class containing all events in the game.
/// </summary>
public class GameEvents : MonoBehaviour
{
    /// <summary>
    /// Current static instance of the class.
    /// </summary>
    public static GameEvents gameEvents;
    
    private void Awake()
    {
        gameEvents = this;
    }

    /// <summary>
    /// Called when the Unit is being destroyed.
    /// </summary>
    public event Action<Unit> OnUnitKilled;
    
    /// <summary>
    /// Called when the option to unite with a World Enemy is chosen as the Dialogue Outcome.
    /// </summary>
    public event Action<JSONReader.UnitJSON> OnUnitUnited;
    /// <summary>
    /// Called when unit receives damage.
    /// </summary>
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
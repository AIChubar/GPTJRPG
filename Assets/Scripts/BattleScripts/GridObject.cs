using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Script for the Grid Tile.
/// </summary>
public class GridObject : MonoBehaviour
{
    /// <summary>
    /// Object that is being hold by this Grid Tile.
    /// </summary>
    [HideInInspector]
    [SerializeField] public GameObject objectHolding;
    
    /// <summary>
    /// Assign <see cref="objectHolding"/> for this Grid Tile.
    /// </summary>
    /// <param name="pref">Prefab for <see cref="Unit"/>.</param>
    /// <param name="ud">Data about the Unit.</param>
    public void AssignObject(GameObject pref, JSONReader.UnitJSON ud) 
    {
        if (pref != null)
        { 
            objectHolding = Instantiate(pref, transform.position, Quaternion.identity);
            var unit = objectHolding.GetComponent<Unit>();
            objectHolding.GetComponent<SpriteRenderer>().sprite = GameManager.gameManager.GetSpriteUnit(ud);
            unit.SetData(ud);
            objectHolding.transform.localScale = new Vector3(1.5f,1.5f, 1);
            objectHolding.transform.Translate(0f, pref.GetComponent<SpriteRenderer>().bounds.size.y/5*2, 0f);
        }
    }
}
  
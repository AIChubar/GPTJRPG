using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for the object representing <see cref="JSONReader.UnitGroup"/> on the <see cref="JSONReader.LevelUnits"/>.
/// </summary>
public class WorldEnemy : MonoBehaviour
{
    [HideInInspector] public JSONReader.UnitGroup groupData;

    [HideInInspector] public JSONReader.DialogueInfo dialogueInfo;

    [HideInInspector] public bool boss;
    /// <summary>
    /// Sets up data about this World Enemy. 
    /// </summary>
    /// <param name="gd">Unit Group data.</param>
    /// <param name="di">Dialogue data.</param>
    /// <param name="isBoss">True if this World Enemy is a main Antagonist.</param>
    public void SetGroup(JSONReader.UnitGroup gd, JSONReader.DialogueInfo di, bool isBoss = false)
    {
        dialogueInfo = di;
        groupData = gd;
        boss = isBoss;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Hero bm))
        {
            GetComponent<Collider2D>().enabled = false;
            
            GameManager.gameManager.gameData.AssignAllyGroup(bm.allyGroup);
            
            GameManager.gameManager.gameData.AssignEnemyGroup(groupData, boss);

            GameManager.gameManager.pauseMenu.ShowUnitInteractionMenu(dialogueInfo, groupData);
            
            Destroy(gameObject, 0.1f);
        }
    }
}
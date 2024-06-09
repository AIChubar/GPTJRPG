using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEnemy : MonoBehaviour
{
    [HideInInspector] public JSONReader.UnitGroup groupData;

    [HideInInspector] public JSONReader.DialogueInfo dialogueInfo;

    [HideInInspector] public bool boss;
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
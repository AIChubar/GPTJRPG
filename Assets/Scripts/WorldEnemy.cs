using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEnemy : MonoBehaviour
{
    public JSONReader.UnitGroup groupData;

    public JSONReader.DialogueInfo dialogueInfo;
    public void SetGroup(JSONReader.UnitGroup gd, JSONReader.DialogueInfo di)
    {
        dialogueInfo = di;
        groupData = gd;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Hero bm))
        {
            GetComponent<Collider2D>().enabled = false;
            
            GameManager.gameManager.gameData.AssignAllyGroup(bm.allyGroup);
            
            GameManager.gameManager.gameData.AssignEnemyGroup(groupData);

            GameManager.gameManager.pauseMenu.ShowBattleStartMenu(dialogueInfo);

            
            Destroy(gameObject, 0.1f);
        }
    }
}
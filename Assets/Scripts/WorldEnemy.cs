using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEnemy : MonoBehaviour
{
    public GroupData groupData;
    public void SetGroup(GroupData gd)
    {
        groupData = gd;
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent(out Hero bm))
        {
            GetComponent<Collider2D>().enabled = false;
            
            GameManager.gameManager.gameData.AssignAllyGroup(bm.allyGroup);
            
            GameManager.gameManager.gameData.AssignEnemyGroup(groupData);
            
            GameManager.gameManager.pauseMenu.ShowBattleStartMenu();

            
            Destroy(gameObject, 0.1f);
        }
    }
}
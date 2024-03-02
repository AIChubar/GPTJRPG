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
            GameManager.gameManager.battleData.AssignAllyGroup(bm.allyGroup);
            
            GameManager.gameManager.battleData.AssignEnemyGroup(groupData);

            SceneController.LoadScene(2, 1, 1, 0.2f);
            
            Destroy(gameObject, 0.5f);
        }
    }
}
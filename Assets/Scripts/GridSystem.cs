using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] public GridObject[] allyGridObjects;
    
    [SerializeField] public GridObject[] enemyGridObjects;

    private void Awake()
    {
        for (int i = 0; i < allyGridObjects.Length; i++)
        {
            if (GameManager.gameManager.battleData.allies == null)
                break;
            allyGridObjects[i].AssignObject(GameManager.gameManager.battleData.battleEnemyPrefab, GameManager.gameManager.battleData.allies.units[i]);
        }
        for (int i = 0; i < GameManager.gameManager.battleData.enemies.units.Count; i++)
        {
            enemyGridObjects[i].AssignObject(GameManager.gameManager.battleData.battleEnemyPrefab, GameManager.gameManager.battleData.enemies.units[i]);
        }
    }
 
}

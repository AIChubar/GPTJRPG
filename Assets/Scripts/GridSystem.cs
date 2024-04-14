using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] public GridObject[] allyGridObjects;
    
    [SerializeField] public GridObject[] enemyGridObjects;

    private void Awake()
    {
        for (int i = 0; i < GameManager.gameManager.gameData.allies.units.Count; i++)
        {
            if (GameManager.gameManager.gameData.allies == null)
                break;
            allyGridObjects[i].AssignObject(GameManager.gameManager.gameData.battleEnemyPrefab, GameManager.gameManager.gameData.allies.units[i]);
        }
        for (int i = 0; i < GameManager.gameManager.gameData.enemies.units.Count; i++)
        {
            enemyGridObjects[i].AssignObject(GameManager.gameManager.gameData.battleEnemyPrefab, GameManager.gameManager.gameData.enemies.units[i]);
        }
    }
 
}

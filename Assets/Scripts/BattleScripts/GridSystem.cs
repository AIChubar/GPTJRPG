using UnityEngine;

/// <summary>
/// Script that holds and assign objects to an existing grid.
/// </summary>
public class GridSystem : MonoBehaviour
{
    /// <summary>
    /// Grid tiles that represents the ally units.
    /// </summary>
    [SerializeField] public GridObject[] allyGridObjects;
    /// <summary>
    /// Grid tiles that represents the enemy units.
    /// </summary>
    [SerializeField] public GridObject[] enemyGridObjects;

    private void Awake()
    {
        for (int i = 0; i < GameManager.gameManager.gameData.allies.units.Count; i++)
        {
            if (GameManager.gameManager.gameData.allies == null)
                break;
            allyGridObjects[i].AssignObject(GameManager.gameManager.gameData.battleUnitPrefab, GameManager.gameManager.gameData.allies.units[i]);
        }
        for (int i = 0; i < GameManager.gameManager.gameData.enemies.units.Count; i++)
        {
            enemyGridObjects[i].AssignObject(GameManager.gameManager.gameData.battleUnitPrefab, GameManager.gameManager.gameData.enemies.units[i]);
        }
    }
 
}

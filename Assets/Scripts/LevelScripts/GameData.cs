
using UnityEngine;
/// <summary>
/// Script for the object that contains units data for the current Battle.
/// </summary>
public class GameData : MonoBehaviour 
{
    /// <summary>
    /// Prefab for the <see cref="Unit"/> object.
    /// </summary>
    [UnityEngine.Tooltip("Prefab for the Unit object.")]
    public GameObject battleUnitPrefab;
    
    /// <summary>
    /// Enemy Units for the current Battle.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Enemy Units for the current Battle.")]
    public JSONReader.UnitGroup enemies = null;

    /// <summary>
    /// Is the current fight against the main Antagonist.
    /// </summary>
    [HideInInspector] public bool isBossFight = false;

    /// <summary>
    /// Ally Units for the current Battle.
    /// </summary>
    [HideInInspector]
    [UnityEngine.Tooltip("Ally Units for the current Battle.")]
    public JSONReader.UnitGroup allies = null;
    
    /// <summary>
    /// Sets up <see cref="enemies"/>.
    /// </summary>
    /// <param name="enemyG">Enemy Group data.</param>
    /// <param name="boss">True if boss Battle.</param>
    public void AssignEnemyGroup(JSONReader.UnitGroup enemyG, bool boss)
    {
        enemies = enemyG;
        isBossFight = boss;
    }
    /// <summary>
    /// Sets up <see cref="allies"/>.
    /// </summary>
    /// <param name="allyG">Ally Group data.</param>
    public void AssignAllyGroup(JSONReader.UnitGroup allyG)
    {
        allies = allyG;
    }
 
}
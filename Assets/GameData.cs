
using UnityEngine;

//[CreateAssetMenu(fileName = "BattleData", menuName = "ScriptableObjects/BattleData", order = 1)]
public class GameData : MonoBehaviour 
{
    public GameObject battleEnemyPrefab;
    
    [HideInInspector]
    public JSONReader.UnitGroup enemies = null;

    [HideInInspector] public bool isBossFight = false;

    [HideInInspector]
    public JSONReader.UnitGroup allies = null;
    
    public void AssignEnemyGroup(JSONReader.UnitGroup enemy, bool boss)
    {
        enemies = enemy;
        isBossFight = boss;
    }
    
    public void AssignAllyGroup(JSONReader.UnitGroup ally)
    {
        allies = ally;
    }
 
}
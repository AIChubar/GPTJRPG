using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;

//[CreateAssetMenu(fileName = "BattleData", menuName = "ScriptableObjects/BattleData", order = 1)]
public class GameData : MonoBehaviour 
{
    public GameObject battleEnemyPrefab;
    
    [HideInInspector]
    public JSONReader.UnitGroup enemies = null;

    [HideInInspector]
    public JSONReader.UnitGroup allies = null;
    
    public void AssignEnemyGroup(JSONReader.UnitGroup enemy)
    {
        enemies = enemy;
    }
    
    public void AssignAllyGroup(JSONReader.UnitGroup ally)
    {
        allies = ally;
    }
 
}
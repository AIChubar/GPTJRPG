using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

//[CreateAssetMenu(fileName = "BattleData", menuName = "ScriptableObjects/BattleData", order = 1)]
public class GameData : MonoBehaviour
{
    public GameObject battleEnemyPrefab;
    
    [HideInInspector]
    public GroupData enemies = null;

    [HideInInspector]
    public GroupData allies = null;
    
    public void AssignEnemyGroup(GroupData enemy)
    {
        enemies = enemy;
    }
    
    public void AssignAllyGroup(GroupData ally)
    {
        allies = ally;
    }
 
}

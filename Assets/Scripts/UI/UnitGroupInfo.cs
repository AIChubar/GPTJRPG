using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Auxiliary script for <see cref="KnowledgeBase"/> UI elements. Used in bestiary to show
/// the <see cref="JSONReader.UnitJSON"/> inside <see cref="JSONReader.UnitGroup"/> which are inside <see cref="JSONReader.LevelUnits"/>.
/// </summary>
public class UnitGroupInfo : MonoBehaviour
{
    /// <summary>
    /// Text UI element that represents name of the group or level.
    /// </summary>
    [SerializeField] private TextMeshProUGUI groupName;
    /// <summary>
    /// Prefab of the <see cref="UnitHUD"/> UI object.
    /// </summary>
    [SerializeField] private GameObject unitInfoPrefab;
    /// <summary>
    /// Prefab of this class UI object.
    /// </summary>
    [SerializeField] private GameObject unitGroupPrefab;
    private List<GameObject> _children = new List<GameObject>();
    /// <summary>
    /// Sets up the UI with informtaion about the units in the group.
    /// </summary>
    /// <param name="group">Unit Group to be shown in this UI object.</param>
    public void SetGroup(JSONReader.UnitGroup group)
    {
        groupName.text = group.groupName;

        foreach (var unit in group.units)
        {
            var unitInfo = Instantiate(unitInfoPrefab, transform);
            _children.Add(unitInfo);
            unitInfo.GetComponent<InfoHUD>().SetHUD(unit);
            Canvas.ForceUpdateCanvases();  
            GetComponent<VerticalLayoutGroup>().enabled = false; 
            GetComponent<VerticalLayoutGroup>().enabled = true;
        }
    }
    /// <summary>
    /// Sets up the UI with informtaion about the unit groups on the level.
    /// </summary>
    /// <param name="level">level which groups are to be shown in this UI object.</param>
    public void SetLevel(JSONReader.LevelUnits level)
    {
        groupName.text = level.levelName;

        foreach (var unit in level.enemyGroups)
        {
            var groupInfo = Instantiate(unitGroupPrefab, transform);
            _children.Add(groupInfo);

            groupInfo.GetComponent<UnitGroupInfo>().SetGroup(unit);
        }
        Canvas.ForceUpdateCanvases();  
        GetComponent<VerticalLayoutGroup>().enabled = false; 
        GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    
}

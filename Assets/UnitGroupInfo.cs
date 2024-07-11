using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitGroupInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI groupName;
    [SerializeField] private GameObject unitInfoPrefab;
    [SerializeField] private GameObject unitGroupPrefab;
    private List<GameObject> _children = new List<GameObject>();
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

    public void ToggleChildren()
    {
        foreach (var child in _children)
        {
            child.SetActive(!child.activeSelf);
        }
        Canvas.ForceUpdateCanvases();  
        GetComponent<VerticalLayoutGroup>().enabled = false; 
        GetComponent<VerticalLayoutGroup>().enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

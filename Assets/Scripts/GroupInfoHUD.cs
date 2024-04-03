using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupInfoHUD : MonoBehaviour 
{
    [SerializeField] public UnitHUD[] GroupHUDs;

    private Hero _hero;
    
    // Start is called before the first frame update
    void Start()
    {
        _hero = GameManager.gameManager.hero;
        for (int i = 0; i < _hero.allyGroup.units.Count; i++)
        {
            GroupHUDs[i].SetHUD(_hero.allyGroup.units[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

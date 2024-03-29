using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class GridObject : MonoBehaviour
{
    //[HideInInspector]
    [SerializeField] public GameObject objectHolding;

    public bool friendly;
    
    //[SerializeField]
    //private GridSystem _gridSystem;

    void Start()
    {
        
    }

    public void AssignObject(GameObject obj, UnitData ud)
    {
        if (obj != null)
        { 
            objectHolding = Instantiate(GameManager.gameManager.battleData.battleEnemyPrefab, transform.position,Quaternion.identity);
            //obj = GameManager.gameManager.MapToBattle(obj, friendly);
            objectHolding.GetComponent<SpriteRenderer>().sprite = GameManager.gameManager.atlas.GetSprite(ud.id.Substring(ud.id.IndexOf('_') + 1));
            objectHolding.GetComponent<Unit>().SetData(ud);
            objectHolding.transform.localScale = new Vector3(1.5f,1.5f, 1);
            objectHolding.transform.Translate(0f, obj.GetComponent<SpriteRenderer>().bounds.size.y/5*2, 0f);
        }
    }

    private void Update()
    {
    }
}
  
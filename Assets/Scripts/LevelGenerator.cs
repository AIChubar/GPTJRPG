using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class  LevelGenerator : MonoBehaviour
{
    [Header("Top left point of spawning area")]
    [SerializeField]
    private Transform topLeft;
    
    [Header("Bottom right point of spawning area")]
    [SerializeField]
    private Transform bottomRight;
    
    public GameObject enemyPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        var unitsGroups = GameManager.gameManager.world.unitsData.levelsUnits[GameManager.gameManager.levelIndex].enemyGroups;
        foreach (var i in unitsGroups)
        {
            var di = GameManager.gameManager.world.dialogues[i.units[0].artisticName];
            SpawnObject(i, di);
        }

    }
    // only enemies
    private void SpawnObject(JSONReader.UnitGroup gd, JSONReader.DialogueInfo di)
    {
        Vector3 pos = new Vector3(Random.Range(topLeft.position.x, bottomRight.position.x),
            Random.Range(bottomRight.position.y, topLeft.position.y), 1f);

        Collider2D Collision = Physics2D.OverlapCircle(pos, 3f, LayerMask.GetMask("Player"));
        int attempts = 0;
        while (Collision)
        {
            attempts += 1;
            if (attempts > 50)
            {
                break;
            }
            pos = new Vector3(Random.Range(topLeft.position.x, bottomRight.position.x),
                Random.Range(bottomRight.position.y, topLeft.position.y), 1f);
            Collision = Physics2D.OverlapCircle(pos, 3f, LayerMask.GetMask("Player"));
        }

        var ob = Instantiate(enemyPrefab, pos, Quaternion.identity);

        ob.transform.parent = transform;
        
        var sp = ob.GetComponent<SpriteRenderer>();

        sp.sprite = GameManager.gameManager.GetSprite(gd.units[0]);
        
        var we = ob.GetComponent<WorldEnemy>();
        
        we.SetGroup(gd, di);

        //ob.transform.SetParent(parent.transform);

        //StartCoroutine(ObjectLife(2, ob, ObjectLifetime ));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

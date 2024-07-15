using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

/// <summary>
/// Script that Generates <see cref="WorldEnemy"/> for the current Level.
/// </summary>
public class  LevelGenerator : MonoBehaviour
{
    [Header("Top left point of spawning area")]
    [SerializeField]
    private Transform topLeft;
    
    [Header("Bottom right point of spawning area")]
    [SerializeField]
    private Transform bottomRight;
    /// <summary>
    /// Prefab for the <see cref="WorldEnemy"/>
    /// </summary>
    [UnityEngine.Tooltip("Prefab for the WorldEnemy")]
    public GameObject enemyPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        var unitsGroups = GameManager.gameManager.world.unitsData.levelsUnits[GameManager.gameManager.levelIndex].enemyGroups;
        foreach (var i in unitsGroups)
        {
            if (!GameManager.gameManager.world.dialogues.TryGetValue(i.units[0].artisticName, out var di))
            {
                di = new JSONReader.DialogueInfo();
            }
            SpawnObject(i, di);
        }

        if (GameManager.gameManager.levelIndex == 2)
        {
            if (!GameManager.gameManager.world.dialogues.TryGetValue(GameManager.gameManager.world.narrativeData
                    .antagonistGroup.units[0].artisticName, out var di))
            {
                di = new JSONReader.DialogueInfo();
            }
            SpawnObject(GameManager.gameManager.world.narrativeData.antagonistGroup, di, true);
        }
    }
    
    private void SpawnObject(JSONReader.UnitGroup gd, JSONReader.DialogueInfo di, bool boss = false)
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

        sp.sprite = GameManager.gameManager.GetSpriteUnit(gd.units[0]);
        if (boss)
        {
            GameManager.gameManager.villain = ob;
            sp.sprite = GameManager.gameManager.GetSpriteUnit(gd.units[0]);
            sp.transform.localScale *= 1.8f;
        }
        
        var we = ob.GetComponent<WorldEnemy>();
        
        we.SetGroup(gd, di, boss);

    }
}

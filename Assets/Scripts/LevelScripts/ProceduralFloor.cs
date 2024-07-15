using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Script randomly changing map tiles to one of the given tiles.
/// </summary>
[RequireComponent(typeof(Tilemap))]
public class ProceduralFloor : MonoBehaviour
{
    [SerializeField]
    private GameObject paletteTileMap;
    
    [Header("Chance for tile to be swapped")]
    [SerializeField]
    private float SwappingChance;

    private Tilemap _floor;
    
    void Start()
    {
        var palette = paletteTileMap.GetComponentInChildren<Tilemap>();
        _floor = gameObject.GetComponent<Tilemap>();
        _floor.CompressBounds();
        for (int i = _floor.cellBounds.xMin; i < _floor.cellBounds.xMax; i++)
        {
            for (int j = _floor.cellBounds.yMin; j < _floor.cellBounds.yMax; j++)
            {
                if (Random.value < SwappingChance)
                {   
                    _floor.SetTile(new Vector3Int(i,j,0), palette.GetTile(new Vector3Int(palette.cellBounds.xMin +Random.Range(1,3), palette.cellBounds.yMin, 0)));
                }
            }
        }
    }
}

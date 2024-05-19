using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PaletteSelection : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> terrainPalettes;
    
    [SerializeField]
    private Tilemap ground;
    [SerializeField]
    private Tilemap obstacle;

    void Start()
    {
        var defaultTilemap = terrainPalettes[0].GetComponentInChildren<Tilemap>();
        var paletteName = GameManager.gameManager.world.levels[GameManager.gameManager.levelIndex].walkableTerrain + "_" + GameManager.gameManager.world.levels[GameManager.gameManager.levelIndex].obstacleTerrain;
        var newTileMapGameObject = terrainPalettes.FirstOrDefault(go =>
            go.name == paletteName);
        if (newTileMapGameObject == null)
        {
            Debug.LogError("Wrong Tile Palette Name: " +  paletteName);
            newTileMapGameObject = terrainPalettes[0];
        }

        var newTileMap = newTileMapGameObject.GetComponentInChildren<Tilemap>();
        if (defaultTilemap.size == newTileMap.size)
        {
            var d = new Dictionary<TileBase, TileBase>();
            var n  = new BoundsInt(newTileMap.origin, newTileMap.size).allPositionsWithin;
            n.Reset();
            foreach (var c in new BoundsInt(defaultTilemap.origin, defaultTilemap.size).allPositionsWithin)
            {
                n.MoveNext();
                var currentTile = defaultTilemap.GetTile(c);
                var newTile = newTileMap.GetTile(n.Current);
                if (currentTile != null && newTile != null)
                {
                    d[currentTile] = newTile;
                }
            }
            foreach (var pair in d)
            {
                if (!pair.Key.Equals(pair.Value))
                    ground.SwapTile(pair.Key, pair.Value);
            }
            foreach (var pair in d)
            {
                if (!pair.Key.Equals(pair.Value))
                    obstacle.SwapTile(pair.Key, pair.Value);
            }
        }
    }
}

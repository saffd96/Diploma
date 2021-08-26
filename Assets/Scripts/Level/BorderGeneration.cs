using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BorderGeneration : MonoBehaviour
{
    [SerializeField] private GameObject borderTile;
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private Transform border;
   
    [SerializeField] Tilemap bgTilemap;
    [SerializeField] Tile[] bgTiles;
    

    private GameObject instance;
    
    private void Start()
    {
        for (var i = 0; i < x+1; i++)
        {
            for (var j = 0; j < y+1; j++)
            {
                var index = Random.Range(0, bgTiles.Length);
                if (i == 0 || j == 0)
                { 
                    instance = Instantiate(borderTile, new Vector2(i-0.5f, j-0.5f), Quaternion.identity);
                    bgTilemap.SetTile(new Vector3Int(i, j, 0), bgTiles[index]);

                }
                else if (i == x || j == y)
                { 
                    instance= Instantiate(borderTile, new Vector2(i+0.5f, j+0.5f), Quaternion.identity);
                }
                else
                {
                    bgTilemap.SetTile(new Vector3Int(i, j, 0), bgTiles[index]);
                }
                instance.transform.parent = border;
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGeneration : MonoBehaviour
{
    [SerializeField] private GameObject borderTile;
    [SerializeField] private int x;
    [SerializeField] private int y;

    [SerializeField] private Transform border;
    private GameObject instance;
    
    private void Start()
    {
        for (var i = 0; i < x+1; i++)
        {
            for (var j = 0; j < y+1; j++)
            {
                if (i == 0 || j == 0)
                { 
                    instance = Instantiate(borderTile, new Vector2(i-0.5f, j-0.5f), Quaternion.identity);
                }
                else if (i == x || j == y)
                { 
                    instance= Instantiate(borderTile, new Vector2(i+0.5f, j+0.5f), Quaternion.identity);
                }
                instance.transform.parent = border;
            }
        }
    }

}

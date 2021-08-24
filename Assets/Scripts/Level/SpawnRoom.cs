// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class SpawnRoom : MonoBehaviour
// {    
//     private Collider2D roomDetection;
//     public LevelGen LevelGeneration;
//     private int rand;
//     void Update()
//     {
//         roomDetection = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask(Layers.Room));
//
//         if (roomDetection == null && LevelGeneration.StopGeneration)
//         {
//             rand = Random.Range(0, LevelGeneration.Rooms.Length);
//             Instantiate(LevelGeneration.Rooms[rand], transform.position, Quaternion.identity);
//             Destroy(gameObject);
//         }
//     }
// }

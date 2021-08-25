using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    private enum Direction
    {
        Top,
        Bottom,
        Forward1,
        Forward2,
    }
    
    [Header("Level setup")]
    [SerializeField] private Transform levelTransform;
    [SerializeField] private Transform[] startingPositions;
    [SerializeField] private float startTimeBtwSpawn = 0.1f;
    [SerializeField] private float offsetAmount;
    [SerializeField] private GameObject exitLvl;
    

    [Header("Rooms")]
    [SerializeField] private GameObject LB;
    [SerializeField] private GameObject RB;
    [SerializeField] private GameObject LR;
    [SerializeField] private GameObject LT;
    [SerializeField] private GameObject RT;

    [Header("Level border settings")]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float maxX;

    private float timeBtwSpawn;

    private int randStartingPositionIndex;

    private Vector2 position;
    private Vector2 newGeneratorPosition;

    private Direction direction;

    private bool stopGeneration;

    private GameObject instance;
    private GameObject tempRoom;

    private void Awake()
    {
        position = transform.position;
        randStartingPositionIndex = Random.Range(0, startingPositions.Length);
        position = startingPositions[randStartingPositionIndex].position;
        CreateRoom(LR); //make starting room
        direction = Direction.Forward1;
        
        GameHandler.StartPosition = position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        GenerateLvl();
    }

    private void GenerateLvl()
    {
        if (stopGeneration) return;

        if (timeBtwSpawn <= 0)
        {
            MoveGenerator();
            timeBtwSpawn = startTimeBtwSpawn;
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

    private void MoveGenerator()
    {
        if (direction == Direction.Top) //move top
        {
            if (position.y < maxY)
            {
                newGeneratorPosition = new Vector2(position.x, position.y + offsetAmount);
                position = newGeneratorPosition;

                CreateRoom(RB);
                direction = Direction.Forward1;
            }
            else //rich upper border of the level. move forward;
            {
                Destroy(tempRoom);
                Instantiate(LR, position, Quaternion.identity);
                direction = Direction.Forward1;
            }
        }
        else if (direction == Direction.Bottom) // move bottom
        {
            if (position.y > minY)
            {
                newGeneratorPosition = new Vector2(position.x, position.y - offsetAmount);
                position = newGeneratorPosition;

                CreateRoom(RT);
                direction = Direction.Forward1;
            }
            else //rich lower border of the level. move forward;
            {
                Destroy(tempRoom);
                CreateRoom(LR);
                direction = Direction.Forward1;
            }
        }
        else if (direction == Direction.Forward1 || direction == Direction.Forward2) //move forward
        {
            if (position.x < maxX)
            {
                newGeneratorPosition = new Vector2(position.x + offsetAmount, position.y);
                position = newGeneratorPosition;

                GetDirection();

                switch (direction)
                {
                    case Direction.Bottom:
                        tempRoom = CreateRoom(LB);
                        
                        break;
                    case Direction.Top:
                        tempRoom = CreateRoom(LT);

                        break;
                    default:
                        tempRoom = CreateRoom(LR);

                        break;
                }
            }
            else // rich end of the level
            {
                CreateExitLvl();
                stopGeneration = true;
            }
        }
    }

    private void GetDirection()
    {
        direction = (Direction)Random.Range(0, Enum.GetNames(typeof(Direction)).Length);
    }

    private void CreateExitLvl()
    {
        var xExitPos = position.x + 3f;
        var hit = Physics2D.Raycast(new Vector2(xExitPos, position.y), Vector2.down, maxY, LayerMask.GetMask(Layers.Ground));
        var yExitPos = hit.point.y;
        var exitPosition = new Vector2(xExitPos, yExitPos);
        Debug.Log(position);
        Debug.Log(exitPosition);
        Instantiate(exitLvl, exitPosition, Quaternion.identity);
    }

    private GameObject CreateRoom(GameObject room)
    {
        instance = Instantiate(room, position, Quaternion.identity);
        instance.transform.parent = levelTransform;

        return instance;
    }
}

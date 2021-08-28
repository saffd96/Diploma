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
        Forward
    }
    
    [Header("Level setup")]
    [SerializeField] private Transform levelTransform;
    [SerializeField] private Transform[] startingPositions;
    [SerializeField] private float offsetAmount;
    

    [Header("Rooms")]
    [SerializeField] private GameObject Enter;
    [SerializeField] private GameObject LB;
    [SerializeField] private GameObject RB;
    [SerializeField] private GameObject LR;
    [SerializeField] private GameObject LT;
    [SerializeField] private GameObject RT;
    [SerializeField] private GameObject Exit;

    [Header("Level border settings")]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float maxX;


    private int randStartingPositionIndex;

    private Vector2 position;
    private Vector2 newGeneratorPosition;

    private Direction direction;

    private bool stopGeneration;

    private GameObject instance;
    private GameObject tempRoom;

    public bool StopGeneration => stopGeneration;
    public Transform LevelTransform => levelTransform;

    private void Awake()
    {
        position = transform.position;
        randStartingPositionIndex = Random.Range(0, startingPositions.Length);
        position = startingPositions[randStartingPositionIndex].position;
        CreateRoom(Enter); //make starting room
        direction = Direction.Forward;
        
        GameHandler.StartPosition = new Vector2(position.x-2, position.y - 2.5f);
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

        MoveGenerator();
    }

    private void MoveGenerator()
    {
        if (direction == Direction.Top) //move top
        {
            if (position.y < maxY)
            {
                newGeneratorPosition = new Vector2(position.x, position.y + offsetAmount);
                position = newGeneratorPosition;

                tempRoom = CreateRoom(RB);
                direction = Direction.Forward;
            }
            else //rich upper border of the level. move forward;
            {
                Destroy(tempRoom);
                CreateRoom(LR);
                direction = Direction.Forward;
            }
        }
        else if (direction == Direction.Bottom) // move bottom
        {
            if (position.y > minY)
            {
                newGeneratorPosition = new Vector2(position.x, position.y - offsetAmount);
                position = newGeneratorPosition;

                tempRoom = CreateRoom(RT);
                direction = Direction.Forward;
            }
            else //rich lower border of the level. move forward;
            {
                Destroy(tempRoom);
                CreateRoom(LR);
                direction = Direction.Forward;
            }
        }
        else if (direction == Direction.Forward) //move forward
        {
            if (position.x < maxX - offsetAmount)
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
                newGeneratorPosition = new Vector2(position.x + offsetAmount, position.y);
                position = newGeneratorPosition;
                CreateRoom(Exit);
                stopGeneration = true;
            }
        }
    }

    private void GetDirection()
    {
        direction = (Direction)Random.Range(0, Enum.GetNames(typeof(Direction)).Length);
    }

    private GameObject CreateRoom(GameObject room)
    {
        instance = Instantiate(room, position, Quaternion.identity);
        instance.transform.parent = levelTransform;

        return instance;
    }
}

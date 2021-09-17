using System;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGeneration : MonoBehaviour
{
    
    private enum Direction
    {
        Top,
        Bottom,
        Forward
    }

    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;

    [Header("Level setup")]
    [SerializeField] private Transform levelTransform;
    [SerializeField] private Transform[] startingPositions;
    [SerializeField] private float offsetAmount;
    [SerializeField]  private AstarPath astarPath;


    [Header("Rooms")]
    [SerializeField] private GameObject Enter;
    [SerializeField] private GameObject LB;
    [SerializeField] private GameObject RB;
    [SerializeField] private GameObject LR;
    [SerializeField] private GameObject LT;
    [SerializeField] private GameObject RT;
    [SerializeField] private GameObject Boss;
    [SerializeField] private GameObject Exit;

    [Header("Level border settings")]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float maxX;

    [Header("BossLevel border settings")]
    [SerializeField] private float bossLevelLenght;

    private int randStartingPositionIndex;

    private float currentMinY;
    private float currentMaxY;
    private float currentMaxX;

    private Vector2 position;
    private Vector2 newGeneratorPosition;

    private Direction direction;

    private GameObject instance;
    private GameObject tempRoom;

    private GameObject player;

    private bool generateBossLvl;

    public bool StopGeneration { get; private set; }
    public Transform LevelTransform => levelTransform;

    private void Awake()
    {
        if (GameHandler.LevelsCompleted >= GameHandler.NeedCastleScenesToPass)
        {
            generateBossLvl = true;
        }

        position = transform.position;

        if (generateBossLvl)
        {
            position = startingPositions[1].position;

            currentMinY = currentMaxY = position.y;
            currentMaxX = bossLevelLenght;
        }
        else
        {
            currentMinY = minY;
            currentMaxY = maxY;
            currentMaxX = maxX;
            randStartingPositionIndex = Random.Range(0, startingPositions.Length);
            position = startingPositions[randStartingPositionIndex].position;
        }

        CreateRoom(Enter);

        direction = Direction.Forward;

        GameHandler.StartPosition = new Vector2(position.x - 2, position.y - 2.5f);

        if (GameHandler.Player != null)
        {
            player = Instantiate(GameHandler.Player, GameHandler.StartPosition, Quaternion.identity);
        }

        cinemachineCamera.Follow = player.transform;
    }

    private void Update()
    {
        GenerateLvl();
    }

    private void GenerateLvl()
    {
        if (StopGeneration) return;

        MoveGenerator();
    }

    private void MoveGenerator()
    {
        if (direction == Direction.Top) //move top
        {
            if (position.y < currentMaxY)
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
            if (position.y > currentMinY)
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
            if (position.x < currentMaxX - offsetAmount)
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

                CreateRoom(!generateBossLvl ? Exit : Boss);
                
                StopGeneration = true;
                astarPath.Scan();
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

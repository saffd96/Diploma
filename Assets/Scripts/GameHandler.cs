using UnityEngine;

public static class GameHandler
{
    public static Vector2 StartPosition;

    private static int levelsCompleted = 0;

    public static int LevelsCompleted => levelsCompleted;

    public static void CompleteLvl()
    {
        levelsCompleted++;
    }
}
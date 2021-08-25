using UnityEngine;

public static class GameHandler
{
    public static Vector2 StartPosition;

    public static int lvlsCompleted = 0;

    public static int LvlsCompleted => lvlsCompleted;

    public static void CompleteLvl()
    {
        lvlsCompleted++;
    }
}
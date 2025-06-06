using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameType
{
    Easy, Fast, Spin, Hard, T20, Custom
}

public static class GameEvents
{
    public static UnityAction OnPlayerOut = delegate { };
    public static UnityAction<Transform> OnPlayerHitBall = delegate { };
    public static UnityAction OnBallMiss = delegate { };
    public static UnityAction OnFielderCaughtBall = delegate { };
    public static UnityAction OnBallHitOutsideGround = delegate { };
    public static UnityAction OnBallSpawn = delegate { };

    public static int runs = 0;
    public static int balls = 0;
    public static bool isSpin = true;
    public static GameType gameType = GameType.Easy;


    public static void HandleHighScore()
    {
        int prevRuns = PlayerPrefs.GetInt("Runs");
        int prevBalls = PlayerPrefs.GetInt("Balls");

        if (prevRuns < runs)
        {
            PlayerPrefs.SetInt("Runs", runs);
            PlayerPrefs.SetInt("Balls", balls);
        }
        else if (prevRuns == runs)
        {
            if (prevBalls > balls)
            {
                PlayerPrefs.SetInt("Balls", balls);
            }
        }

        runs = 0;
        balls = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class CurrentScore
{
    private static int _score;

    public static int GetScore()
    {
        return _score;
    }

    public static void SetScore(int score)
    {
        _score = score;
    }
}

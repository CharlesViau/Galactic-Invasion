using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreBoardController : MonoBehaviour
{
    [SerializeField] private List<TableScore> tableScoreList;

    private void Awake()
    {
        StartCoroutine(Scoreboard.Instance.GetScores(scores => { WriteScore(scores); }));
        
    }

    private void WriteScore(Scoreboard.Scores scores)
    {
        for (int i = 0; i < scores.scores.Count; i++)
        {
            tableScoreList[i].Name.text = scores.scores[i].player;
            tableScoreList[i].Score.text = scores.scores[i].score.ToString();
        }
    }
}


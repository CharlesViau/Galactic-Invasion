using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreBoardController : MonoBehaviour
{
    [SerializeField] private List<TableScore> tableScoreList;
    [SerializeField] private Button refreshButton;


    private void Awake()
    {
        refreshButton.onClick.AddListener(Refresh);
        Refresh();
    }
    
    public void Refresh()
    {
        StartCoroutine(Scoreboard.Instance.GetScores(scores => { WriteScore(scores); }));
    }

    private void WriteScore(Scoreboard.Scores scores)
    {
        int count = Mathf.Min(scores.scores.Count, tableScoreList.Count);
        for (int i = 0; i < count; i++)
        {
            tableScoreList[i].Name.text = scores.scores[i].player;
            tableScoreList[i].Score.text = scores.scores[i].score.ToString();
        }
    }
}


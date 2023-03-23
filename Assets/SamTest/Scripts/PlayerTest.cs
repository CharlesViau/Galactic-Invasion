using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public string player
    {
        get;
        private set;
    }
    private int _score;


    private void Reset()
    {
        _score = 0;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Reset();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _score++;
            Debug.Log("Score: " + _score);
        }

        if (Input.GetKeyDown(KeyCode.G))
            StartCoroutine(Scoreboard.Instance.GetScores(scores => { Debug.Log(scores.scores); }));
        if (Input.GetKeyDown(KeyCode.S))
            StartCoroutine(Scoreboard.Instance.SetScore(player, _score, score => { Debug.Log(score); }));
    }
}
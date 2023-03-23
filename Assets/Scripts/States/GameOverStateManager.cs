using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverStateManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TMP_InputField inputField;
    public Button submitButton;
    public GameObject gameOverPanel;
    public GameObject scoreboardPanel;
    public GameObject scoreRowPrefab;
    public Transform content;
    public Button replayButton;
    public Button mainMenuButton;
    private int _score;
    private string _username;

    // Start is called before the first frame update
    public void Start()
    {
        submitButton.interactable = false;

        _score = CurrentScore.GetScore();
        scoreText.text = $"Score: {_score}";

        inputField.onValueChanged.AddListener(OnUsernameChanged);
        submitButton.onClick.AddListener(SubmitScore);
    }

    private void OnUsernameChanged(string username)
    {
        _username = username.ToUpper();
        submitButton.interactable = username.Length == 3;
    }

    private void SubmitScore()
    {
        submitButton.interactable = false;
        StartCoroutine(Scoreboard.Instance.SetScore(_username, _score, score =>
        {
            RefreshScoreboard(score.rank - 5, 9);
            ShowScoreboardPanel();
        }));
    }

    private void ShowScoreboardPanel()
    {
        gameOverPanel.SetActive(false);
        scoreboardPanel.SetActive(true);
    }

    private void RefreshScoreboard(int offset, int limit)
    {
        StartCoroutine(Scoreboard.Instance.GetScores(scores =>
        {
            foreach (var score in scores.scores)
            {
                var scoreRow = Instantiate(scoreRowPrefab, content);
                scoreRow.GetComponent<TextMeshProUGUI>().text =
                    $"{score.rank}\t{score.player}\t\t{score.score}";
            }
        }, offset, limit));
    }
}
using System.Linq;
using Enemies;
using Projectiles;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public GameObject buttonsPanel;
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
            RefreshScoreboard(score);
            ShowScoreboardPanel();
        }));
    }

    private void ShowScoreboardPanel()
    {
        gameOverPanel.SetActive(false);
        scoreboardPanel.SetActive(true);
        buttonsPanel.SetActive(true);
    }

    private void RefreshScoreboard(Scoreboard.Score newScore)
    {
        StartCoroutine(Scoreboard.Instance.GetScores(scores =>
        {
            foreach (var score in scores.scores)
            {
                var scoreRow = Instantiate(scoreRowPrefab, content);
                scoreRow.GetComponent<TextMeshProUGUI>().text =
                    $"{score.rank}\t\t{score.player}\t\t{score.score}";
            }

            if (newScore.rank <= scores.scores.Last().rank) return;
            var newScoreRow = Instantiate(scoreRowPrefab, content);
            newScoreRow.GetComponent<TextMeshProUGUI>().text =
                $"{newScore.rank}\t\t{newScore.player}\t\t{newScore.score}";
        }));
    }

    public void ReplayGame()
    {
        EnemyManager.Instance.Clear();
        ProjectileManager.Instance.Clear();
        Controller.Instance.Reset();
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
using Motherbase;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayerScoreUI : MonoBehaviour
    {
        public GameObject player;
        private PlayerScore _playerScore;
        private TextMeshProUGUI _scoreText;

        private void Start()
        {
            _playerScore = player.GetComponent<PlayerScore>();
            _scoreText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _scoreText.text = $"Score: {_playerScore.GetScore()}";
        }
    }
}
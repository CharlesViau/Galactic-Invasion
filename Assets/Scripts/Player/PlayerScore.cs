using UnityEngine;

namespace Motherbase
{
    public class PlayerScore : MonoBehaviour
    {
        private float _score;
        public static PlayerScore Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            _score = 0;
            CurrentScore.SetScore((int)_score);
        }

        private void Update()
        {
            if (!Controller.Instance.gameStarted) return;
            _score += Time.deltaTime;
            CurrentScore.SetScore((int)_score);
        }

        public void AddScore(int score)
        {
            _score += score;
            CurrentScore.SetScore((int)_score);
        }

        public int GetScore()
        {
            return (int)_score;
        }
    }
}
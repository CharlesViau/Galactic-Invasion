using UnityEngine;

namespace Motherbase
{
    public class PlayerScore : MonoBehaviour
    {
        private float _score;

        private void Start()
        {
            _score = 0;
            CurrentScore.SetScore((int)_score);
        }

        private void Update()
        {
            if (Controller.Instance.gameStarted)
            {
                _score += Time.deltaTime;
                CurrentScore.SetScore((int)_score);
            }
        }

        public int GetScore()
        {
            return (int)_score;
        }
    }
}
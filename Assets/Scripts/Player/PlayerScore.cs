using UnityEngine;

namespace Motherbase
{
    public class PlayerScore : MonoBehaviour
    {
        private float _score;

        private void Start()
        {
            _score = 0;
        }

        private void Update()
        {
            _score += Time.deltaTime;
        }

        public int GetScore()
        {
            return (int)_score;
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ScoreCounter : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _imageCheck;
        [SerializeField] private Image _imageCubes;
        
        private int _maxNumber;

        public event Action Filled;

        public void SetParameters(int maxNumber)
        {
            _maxNumber = maxNumber;
            _slider.maxValue = _maxNumber;
            _slider.value = 0f;
        }
        
        public void AddScore(int score)
        {
            _slider.value += score;

            CalculateScore();
        }

        private void CalculateScore()
        {
            if (_slider.value >= _maxNumber)
            {
                _imageCheck.gameObject.SetActive(true);
                _imageCubes.gameObject.SetActive(false);
                Filled?.Invoke();
            }
        }
    }
}

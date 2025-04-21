using System;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Timer
{
    public class Timeline : MonoBehaviour
    {
        [SerializeField] private int _time;
        [SerializeField] private Slider _slider;
        [SerializeField] private Image _imageDefeat;
        [SerializeField] private Image _imageTime;
        [SerializeField] private ButtonDecreaseTimer _button;

        private float _currentTime;
        private bool _isTimerRunning;
        private bool _isSpent;

        public event Action Stoped;

        private void Start()
        {
            int multiplier = 5;
            int extraTime = YandexGame.savesData.ExtraTime * multiplier;
            _time += extraTime;

            _slider.maxValue = _time;
            _slider.value = 0f;
            _currentTime = 0f;
        }

        private void OnEnable()
        {
            _button.Clicked += OnButtonClicked;

            YandexGame.OpenVideoEvent += OnVideoStarted;
            YandexGame.CloseVideoEvent += OnVideoClosed;
            YandexGame.RewardVideoEvent += OnSetReward;
        }

        private void OnDisable()
        {
            _button.Clicked -= OnButtonClicked;

            YandexGame.OpenVideoEvent -= OnVideoStarted;
            YandexGame.CloseVideoEvent -= OnVideoClosed;
            YandexGame.RewardVideoEvent -= OnSetReward;
        }

        private void Update()
        {
            if (_isTimerRunning)
            {
                _currentTime += Time.deltaTime;
                _slider.value = _currentTime;

                CalculateScore();
                TurnOnButton();
            }
        }

        public void AddTime() =>
            _currentTime++;

        public void RemoveTime(int value = 1) =>
            _currentTime = Math.Clamp(_currentTime - value, 0, _time);

        public void StartTimer() =>
            _isTimerRunning = true;

        public void StopTimer() =>
            _isTimerRunning = false;

        private void CalculateScore()
        {
            if (_slider.value >= _slider.maxValue)
            {
                _imageDefeat.gameObject.SetActive(true);
                _imageTime.gameObject.SetActive(false);
                _button.gameObject.SetActive(false);
                Stoped?.Invoke();
                StopTimer();
            }
        }

        private void TurnOnButton()
        {
            float percent = 0.9f;

            if (_currentTime >= _time * percent && _isSpent == false)
            {
                _button.gameObject.SetActive(true);
                _isSpent = true;
            }
        }

        private void OnButtonClicked()
        {
            if (YandexGame.savesData.IsBuy == false)
            {
                StopTimer();
                YandexGame.RewVideoShow(0);
            }
            else
            {
                OnSetReward(0);
            }

            _button.gameObject.SetActive(false);
        }

        private void OnVideoStarted() =>
            Time.timeScale = 0;

        private void OnVideoClosed()
        {
            Time.timeScale = 1;
            StartTimer();
        }

        private void OnSetReward(int index)
        {
            OnVideoClosed();
            int extraTime = _time / 2;
            RemoveTime(extraTime);
        }
    }
}
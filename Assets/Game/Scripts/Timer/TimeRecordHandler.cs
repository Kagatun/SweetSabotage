using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Timer
{
    public class TimeRecordHandler : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private TextMeshProUGUI _textCurrentTime;

        private float _currentTime;
        private bool _isCount = true;
        private int _maxIndexLevel = 70;
        private string _leaderboard = "LevelsCompleteTime";

        private void Start()
        {
            if (YandexGame.savesData.Times == null || YandexGame.savesData.Times.Count == 0)
            {
                YandexGame.savesData.Times = new List<float>(Enumerable.Repeat(0f, _maxIndexLevel + 1));
            }

            int indexLevel = SceneManager.GetActiveScene().buildIndex;

            if (indexLevel < YandexGame.savesData.Times.Count)
            {
                float previousTime = YandexGame.savesData.Times[indexLevel];

                if (previousTime > 0)
                    _text.text = FormatTime(previousTime);
            }

            _textCurrentTime.text = FormatTime(_currentTime);
        }

        private void Update()
        {
            if (_isCount)
            {
                _currentTime += Time.deltaTime;
                _textCurrentTime.text = FormatTime(_currentTime);
            }
        }

        public void StartTimer() =>
            _isCount = true;

        public void StopTimer() =>
            _isCount = false;

        public void CalculateRecord()
        {
            _isCount = false;
            float defaultStartTime = 0;

            int currentIndexLevel = SceneManager.GetActiveScene().buildIndex;

            while (currentIndexLevel >= YandexGame.savesData.Times.Count)
                YandexGame.savesData.Times.Add(defaultStartTime);

            float previousTime = YandexGame.savesData.Times[currentIndexLevel];

            if (previousTime > _currentTime || previousTime == 0)
            {
                YandexGame.savesData.Times[currentIndexLevel] = _currentTime;
                YandexGame.SaveProgress();
                _image.gameObject.SetActive(true);
            }

            _text.text = FormatTime(_currentTime);

            RecordTotalTimeCompleteGame();
        }

        private string FormatTime(float time)
        {
            int secondsInMinute = 60;
            int secondsInHour = 3600;

            int hours = (int)(time / secondsInHour);
            int minutes = (int)((time % secondsInHour) / secondsInMinute);
            int seconds = (int)(time % secondsInMinute);

            return $"{hours}:{minutes:D2}:{seconds:D2}";
        }

        private void RecordTotalTimeCompleteGame()
        {
            float totalTime = 0;
            int correctionNumber = 10000;

            for (int i = 0; i < YandexGame.savesData.Times.Count; i++)
                totalTime += YandexGame.savesData.Times[i];

            int completedLevels = YandexGame.savesData.LevelIndex - 1;
            int score = completedLevels * correctionNumber - (int)totalTime;

            YandexGame.NewLeaderboardScores(_leaderboard, score);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using ManagementUtilities;
using Spawner;
using Timer;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using YG;

namespace Game
{
    public class GameEndManager : MonoBehaviour
    {
        [SerializeField] private List<SpawnerGeese> _spawnersGeese;
        [SerializeField] private CellCounter _cellCounter;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private Timeline _timeline;
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private Image _panelVictory;
        [SerializeField] private Image _panelDefeat;
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _musicVictory;
        [SerializeField] private AudioSource _musicDefeat;
        [SerializeField] private FigureDragger _figureDragger;
        [SerializeField] private TimeRecordHandler _timeRecordHandler;
        [SerializeField] private GoldCounter _goldCounter;

        private WaitForSeconds _wait;
        private float _time = 3;
        private bool _isEnd;

        private void Awake()
        {
            _wait = new WaitForSeconds(_time);
        }

        private void OnEnable()
        {
            _scoreCounter.Filled += ActivatePanelVictory;
            _timeline.Stoped += ActivatePanelDefeat;
        }

        private void OnDisable()
        {
            _scoreCounter.Filled -= ActivatePanelVictory;
            _timeline.Stoped -= ActivatePanelDefeat;
        }

        private void ActivatePanelVictory()
        {
            _timeRecordHandler.StopTimer();

            if (_isEnd == false)
            {
                if (_cellCounter.gameObject.activeSelf == false ||
                    (_cellCounter.gameObject.activeSelf && _cellCounter.IsCountComplete))
                {
                    _musicVictory.Play();
                    OpenNextLevel();
                    StartCoroutine(StartEnablePanel(_panelVictory));
                    TurnOffObjects(_musicVictory);
                    _timeRecordHandler.CalculateRecord();
                }
                else
                {
                    ActivatePanelDefeat();
                }
            }
        }

        private void DeleteGeese()
        {
            for (int i = 0; i < _spawnersGeese.Count; i++)
                _spawnersGeese[i].RemoveAllGoose();
        }

        private void ActivatePanelDefeat()
        {
            if (_isEnd)
                return;

            StartCoroutine(StartEnablePanel(_panelDefeat));
            TurnOffObjects(_musicDefeat);
        }

        private void TurnOffObjects(AudioSource music)
        {
            music.Play();
            _music.Stop();
            _isEnd = true;
            _figureDragger.ClearFigure();
        }

        private IEnumerator StartEnablePanel(Image panel)
        {
            _inputDetector.enabled = false;
            _timeline.StopTimer();
            DeleteGeese();

            yield return _wait;

            panel.gameObject.SetActive(true);
            _goldCounter.ShowGold();
            Time.timeScale = 0;
        }

        private void OpenNextLevel()
        {
            if (YandexGame.savesData.LevelNumber + 1 == YandexGame.savesData.LevelIndex)
                YandexGame.savesData.LevelIndex++;

            YandexGame.SaveProgress();
        }
    }
}
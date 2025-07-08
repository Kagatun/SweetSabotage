using System.Collections.Generic;
using Figure;
using ManagementUtilities;
using Timer;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Game
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Button _buttonHintTimer;
        [SerializeField] private Button _buttonHintTimerMobile;

        [SerializeField] private Button _buttonProgress;
        [SerializeField] private Button _buttonProgressMobile;

        [SerializeField] private Button _buttonStun;
        [SerializeField] private Button _buttonStunMobile;

        [SerializeField] private Button _buttonStartGame;

        [SerializeField] private Timeline _timeline;
        [SerializeField] private TimeRecordHandler _timeRecordHandler;
        [SerializeField] private ScoreCounter _scoreCounter;
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private FigureDragger _figureDragger;
        [SerializeField] private RemoverFigures _removerFigures;
        [SerializeField] private DeafeningBeam _deafeningBeam;

        [SerializeField] private Image _imageGreeting;

        [SerializeField] private Image _imageHintFigureDrag;
        [SerializeField] private Image _imageHintFigureDragMobile;

        [SerializeField] private Image _imageHintFigureRemove;
        [SerializeField] private Image _imageHintFigureRemoveMobile;

        [SerializeField] private Image _imageHintRemoveInfo;
        [SerializeField] private Image _imageHintRemoveInfoMobile;

        [SerializeField] private Image _image;
        [SerializeField] private Image _imageMobile;

        [SerializeField] private Image _imageEnd;

        [SerializeField] private List<GameObject> _gameObjects;

        private TeleporterFigure[] _figures;
        private int numberHint;

        private void Start()
        {
            foreach (var _gameObject in _gameObjects)
                _gameObject.SetActive(false);
            
            float timeEnableHints = 0.2f;
            Invoke(nameof(EnableGreeting), timeEnableHints);
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(EnableFirstHint);
            _buttonHintTimer.onClick.AddListener(EnableFourthHint);
            _buttonHintTimerMobile.onClick.AddListener(EnableFourthHint);

            _buttonProgress.onClick.AddListener(EnableFifthHint);
            _buttonProgressMobile.onClick.AddListener(EnableFifthHint);

            _buttonStun.onClick.AddListener(EnableSeventhHint);
            _buttonStunMobile.onClick.AddListener(EnableSeventhHint);

            _buttonStartGame.onClick.AddListener(StartGame);

            _removerFigures.Fined += EnableThirdHint;
            _deafeningBeam.Stuned += EnableSixthHint;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(EnableFirstHint);
            _buttonHintTimer.onClick.RemoveListener(EnableFourthHint);
            _buttonHintTimerMobile.onClick.RemoveListener(EnableFourthHint);

            _buttonProgress.onClick.RemoveListener(EnableFifthHint);
            _buttonProgressMobile.onClick.RemoveListener(EnableFifthHint);

            _buttonStun.onClick.RemoveListener(EnableSeventhHint);
            _buttonStunMobile.onClick.RemoveListener(EnableSeventhHint);

            _buttonStartGame.onClick.RemoveListener(StartGame);

            _removerFigures.Fined -= EnableThirdHint;
            _deafeningBeam.Stuned -= EnableSixthHint;
        }

        private void SwitchPanelByDeviceType(Image image, Image imageMobile, bool isOpen)
        {
            if (YandexGame.savesData.IsDesktop)
                image.gameObject.SetActive(isOpen);
            else
                imageMobile.gameObject.SetActive(isOpen);
        }

        private void EnableGreeting()
        {
            _timeRecordHandler.StopTimer();
            _timeline.StopTimer();
            _imageGreeting.gameObject.SetActive(true);
        }

        private void EnableFirstHint()
        {
            SwitchPanelByDeviceType(_imageHintFigureDrag, _imageHintFigureDragMobile, true);

            _figureDragger.gameObject.SetActive(true);
            _inputDetector.enabled = true;

            _figures = FindObjectsOfType<TeleporterFigure>();

            foreach (var figure in _figures)
                figure.Used += EnableSecondHint;
        }

        private void EnableSecondHint()
        {
            foreach (var figure in _figures)
                figure.Used -= EnableSecondHint;

            SwitchPanelByDeviceType(_imageHintFigureDrag, _imageHintFigureDragMobile, false);

            _figureDragger.gameObject.SetActive(false);
            _removerFigures.gameObject.SetActive(true);

            SwitchPanelByDeviceType(_imageHintFigureRemove, _imageHintFigureRemoveMobile, true);
        }

        private void EnableThirdHint(Color color)
        {
            _removerFigures.gameObject.SetActive(false);

            SwitchPanelByDeviceType(_imageHintFigureRemove, _imageHintFigureRemoveMobile, false);
            SwitchPanelByDeviceType(_imageHintRemoveInfo, _imageHintRemoveInfoMobile, true);

            _removerFigures.Fined -= EnableThirdHint;
        }

        private void EnableFourthHint()
        {
            _timeline.gameObject.SetActive(true);
            _timeline.StopTimer();
        }

        private void EnableFifthHint()
        {
            _scoreCounter.gameObject.SetActive(true);
        }

        private void EnableSixthHint()
        {
            _deafeningBeam.gameObject.SetActive(false);

            SwitchPanelByDeviceType(_image, _imageMobile, false);

            _imageEnd.gameObject.SetActive(true);

            _deafeningBeam.Stuned -= EnableSixthHint;
        }

        private void EnableSeventhHint()
        {
            _deafeningBeam.gameObject.SetActive(true);
        }

        private void StartGame()
        {
            _timeRecordHandler.StartTimer();
            _timeline.StartTimer();
            _inputDetector.enabled = true;
            _figureDragger.gameObject.SetActive(true);
            _removerFigures.gameObject.SetActive(true);
            _deafeningBeam.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
using InterfaceUI;
using ManagementUtilities;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Game
{
    public class PauseHandler : ButtonHandler
    {
        [SerializeField] private InputDetector _inputDetector;
        [SerializeField] private Image _panelPause;
        [SerializeField] private Image _panelInterface;
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _soundMenu;
        [SerializeField] private Button _buttonPlay;

        protected override void OnEnableAction()
        {
            _inputDetector.TabPressed += OnButtonClick;
            _buttonPlay.onClick.AddListener(OnButtonClick);
        }

        protected override void OnDisableAction()
        {
            _inputDetector.TabPressed -= OnButtonClick;
            _buttonPlay.onClick.RemoveListener(OnButtonClick);
        }

        protected override void OnButtonClick()
        {
            if (_panelPause.gameObject.activeSelf == false)
                PauseGame();
            else
                UnPauseGame();
        }

        private void UnPauseGame()
        {
            _music.UnPause();
            Time.timeScale = 1.0f;
            _inputDetector.SetStatusNotPressed();
            _panelPause.gameObject.SetActive(false);
            _panelInterface.gameObject.SetActive(true);
        }

        private void PauseGame()
        {
            if (YandexGame.savesData.IsBuy == false)
                YandexGame.FullscreenShow();

            _panelPause.gameObject.SetActive(true);
            _panelInterface.gameObject.SetActive(false);
            _inputDetector.SetStatusPressed();
            Time.timeScale = 0;
            _soundMenu.Play();
            _music.Pause();
        }
    }
}